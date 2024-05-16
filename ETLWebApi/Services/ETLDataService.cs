using ETLWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic.FileIO;

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

        public async Task ImportData()
        {
            string filePath = "sample-cab-data.csv";
            string[] columnsToRead = { "tpep_pickup_datetime", "tpep_dropoff_datetime", "passenger_count", "trip_distance", "store_and_fwd_flag", "PULocationID",
        "DOLocationID", "fare_amount", "tip_amount" }; // Specify columns you want to read

            List<string[]> data = ReadColumnsFromCSV(filePath, columnsToRead);

            int batchSize = 1000; // Set your preferred batch size

            List<ETLData> etlDataList = new List<ETLData>();

            foreach (var record in data)
            {
                ETLData etlData = new ETLData
                {
                    tpep_pickup_datetime = Convert.ToDateTime(record[0]),
                    tpep_dropoff_datetime = Convert.ToDateTime(record[1]),
                    passenger_count = Convert.ToInt32(record[2]),
                    trip_distance = Convert.ToDouble(record[3]),
                    store_and_fwd_flag = record[4],
                    PULocationID = Convert.ToInt32(record[5]),
                    DOLocationID = Convert.ToInt32(record[6]),
                    fare_amount = Convert.ToDouble(record[7]),
                    tip_amount = Convert.ToDouble(record[8])
                };

                etlDataList.Add(etlData);

                if (etlDataList.Count >= batchSize)
                {
                    await InsertBatchAsync(etlDataList, _context);
                    etlDataList.Clear();
                }
            }

            if (etlDataList.Any())
            {
                await InsertBatchAsync(etlDataList, _context);
            }
        }

        private static List<string[]> ReadColumnsFromCSV(string filePath, string[] columnsToRead)
        {
            var result = new List<string[]>();

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
                    string[] selectedFields = new string[columnIndices.Count];
                    for (int i = 0; i < columnIndices.Count; i++)
                    {
                        selectedFields[i] = fields[columnIndices[i]];
                    }
                    result.Add(selectedFields);
                }
            }

            return result;
        }

        private async Task InsertBatchAsync(List<ETLData> etlDataList, ETLDbContext dbContext)
        {
            try
            {
                await dbContext.ETLDatas.AddRangeAsync(etlDataList);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                throw;
            }
        }

        public async Task<int> FindPULocationId()
        {
            var result = await _context.ETLDatas
                .GroupBy(d => d.PULocationID)
                .Select(g => new
                {
                    PULocationID = g.Key,
                    AverageTipAmount = g.Average(d => d.tip_amount)
                })
                .OrderByDescending(x => x.AverageTipAmount)
                .FirstOrDefaultAsync();

            return result?.PULocationID ?? -1; // Return -1 if no data found
        }

        public async Task<IEnumerable<ETLData>> FindLongestTripDistance()
        {
            var result = await _context.ETLDatas
                .OrderByDescending(d => d.trip_distance)
                .Take(100)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ETLData>> FindLongestTimeSpentTravelling()
        {
            var result = await _context.ETLDatas
                .Select(d => new
                {
                    ETLData = d,
                    TimeSpentTraveling = (d.tpep_dropoff_datetime - d.tpep_pickup_datetime).TotalSeconds
                })
                .OrderByDescending(d => d.TimeSpentTraveling)
                .Take(100)
                .Select(d => d.ETLData)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ETLData>> GetDatas(int pULocationId)
        {
            var result = await _context.ETLDatas
                .Where(d => d.PULocationID == pULocationId)
                .ToListAsync();

            return result;
        }
    }
}