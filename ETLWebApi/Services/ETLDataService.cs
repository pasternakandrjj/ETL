using CsvHelper;
using ETLWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace ETLWebApi.Services
{
    public class ETLDataService : IETLDataService
    {
        private readonly ETLDbContext _context;
        private readonly IMemoryCache _cache;

        public ETLDataService(DbContextOptions<ETLDbContext> options, IMemoryCache cache)
        {
            _context = new ETLDbContext(options);
            _cache = cache;
        }

        public async Task ImportDataAsync()
        {
            string filePath = "sample-cab-data.csv";
            string[] columnsToRead = { "tpep_pickup_datetime", "tpep_dropoff_datetime", "passenger_count", "trip_distance", "store_and_fwd_flag", "PULocationID",
                "DOLocationID", "fare_amount", "tip_amount" }; // Specify columns you want to read
            List<ETLData> data = new List<ETLData>();
            try
            {
                data = ReadColumnsFromCSV(filePath, columnsToRead);
            } 
            catch (Exception ex)
            {
                throw new Exception("Something wrong with the file!");
            }
            // Write duplicates to a file and remove them
            WriteAndRemoveDuplicates(data);

            // Insert data into the database in batches
            await InsertDataInBatchesAsync(data);
        }

        private List<ETLData> ReadColumnsFromCSV(string filePath, string[] columnsToRead)
        {
            var result = new List<ETLData>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Read header to get column indices
                string[] headers = parser.ReadFields();
                Dictionary<string, int> columnIndexMap = new Dictionary<string, int>();
                for (int i = 0; i < headers.Length; i++)
                {
                    columnIndexMap[headers[i]] = i;
                }

                // Find indices of columns to read
                List<int> columnIndices = new List<int>();
                foreach (var column in columnsToRead)
                {
                    if (columnIndexMap.ContainsKey(column))
                    {
                        columnIndices.Add(columnIndexMap[column]);
                    }
                    else
                    {
                        // Handle column not found error
                        Console.WriteLine($"Column '{column}' not found.");
                    }
                }

                // Read data
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    ETLData etlData = new ETLData();

                    for (int i = 0; i < columnIndices.Count; i++)
                    {
                        switch (columnsToRead[i])
                        {
                            case "tpep_pickup_datetime":
                            case "tpep_dropoff_datetime":
                                DateTime.TryParseExact(fields[columnIndices[i]], "M/d/yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetime);
                                if (columnsToRead[i] == "tpep_pickup_datetime")
                                    etlData.PickupDatetime = ConvertToUtc(datetime);
                                else
                                    etlData.DropoffDatetime = ConvertToUtc(datetime);
                                break;
                            case "passenger_count":
                                if (int.TryParse(fields[columnIndices[i]], out int passengerCount))
                                    etlData.PassengerCount = passengerCount;
                                break;
                            case "trip_distance":
                                if (double.TryParse(fields[columnIndices[i]], out double tripDistance))
                                    etlData.TripDistance = tripDistance;
                                break;
                            case "store_and_fwd_flag":
                                etlData.StoreAndFwdFlag = fields[columnIndices[i]] == "N" ? "No" : "Yes";
                                break;
                            case "PULocationID":
                                if (int.TryParse(fields[columnIndices[i]], out int puLocationID))
                                    etlData.PULocationID = puLocationID;
                                break;
                            case "DOLocationID":
                                if (int.TryParse(fields[columnIndices[i]], out int doLocationID))
                                    etlData.DOLocationID = doLocationID;
                                break;
                            case "fare_amount":
                                if (double.TryParse(fields[columnIndices[i]], out double fareAmount))
                                    etlData.FareAmount = fareAmount;
                                break;
                            case "tip_amount":
                                if (double.TryParse(fields[columnIndices[i]], out double tipAmount))
                                    etlData.TipAmount = tipAmount;
                                break;
                                // Add cases for other columns if needed
                        }
                    }

                    TrimStringProperties(etlData);
                    result.Add(etlData);
                }
            }

            return result;
        }

        private DateTime ConvertToUtc(DateTime estDateTime)
        {
            // Convert EST to UTC
            TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(estDateTime, estTimeZone);
        }

        private void WriteAndRemoveDuplicates(List<ETLData> data)
        {
            var groupedData = data.GroupBy(x => new { x.PickupDatetime, x.DropoffDatetime, x.PassengerCount });

            var duplicates = groupedData.Where(g => g.Count() > 1).SelectMany(g => g).ToList();

            if (duplicates.Any())
            {
                using (var writer = new StreamWriter("duplicates.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(duplicates);
                }

                // Remove duplicates from the original data list
                foreach (var group in groupedData)
                {
                    if (group.Count() > 1)
                    {
                        // Keep only the first record (remove duplicates)
                        var firstRecord = group.First();
                        data.RemoveAll(d => d.PickupDatetime == firstRecord.PickupDatetime &&
                                             d.DropoffDatetime == firstRecord.DropoffDatetime &&
                                             d.PassengerCount == firstRecord.PassengerCount);
                    }
                }
            }
        }

        private async Task InsertDataInBatchesAsync(List<ETLData> data)
        {
            int batchSize = 1000; // Set your preferred batch size

            for (int i = 0; i < data.Count; i += batchSize)
            {
                var batch = data.Skip(i).Take(batchSize).ToList();
                await InsertBatchAsync(batch);
            }
        }

        private async Task InsertBatchAsync(List<ETLData> etlDataList)
        {
            try
            {
                await _context.ETLDatas.AddRangeAsync(etlDataList);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                throw;
            }
        }

        private void TrimStringProperties(ETLData etlData)
        {
            etlData.StoreAndFwdFlag = etlData.StoreAndFwdFlag?.Trim();
            // Trim other string properties as needed
        }

        public async Task<int> FindPULocationIdAsync()
        {
            var result = await _context.ETLDatas
                .GroupBy(d => d.PULocationID)
                .Select(g => new
                {
                    PULocationID = g.Key,
                    AverageTipAmount = g.Average(d => d.TipAmount)
                })
                .OrderByDescending(x => x.AverageTipAmount)
                .FirstOrDefaultAsync();

            return result?.PULocationID ?? -1; // Return -1 if no data found
        }

        public async Task<IEnumerable<ETLData>> FindLongestTripDistanceAsync()
        {
            var result = await _context.ETLDatas
                .OrderByDescending(d => d.TripDistance)
                .Take(100)
                .ToListAsync();

            return result;
        }

        public IEnumerable<ETLData> FindLongestTimeSpentTravelling()
        {
            var result = _context.ETLDatas
                .AsEnumerable() // Switch to LINQ to Objects
                .OrderByDescending(e => (e.DropoffDatetime - e.PickupDatetime).TotalSeconds)
                .Take(100)
                .ToList();

            return result;
        }

        public async Task<IEnumerable<ETLData>> GetDatasAsync(int pULocationId)
        {
            var result = await _context.ETLDatas
                .Where(d => d.PULocationID == pULocationId)
                .ToListAsync();

            return result;
        }
    }
}