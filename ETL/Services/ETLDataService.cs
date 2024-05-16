using ETL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory; 

namespace ETL.Services
{
    public class ETLDataService: IETLDataService
    {
        private readonly ETLDbContext _context;
        private readonly IMemoryCache _cache;

        public ETLDataService(ETLDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<ETLData>> GetRectangles(double a, double b)
        {
            var cacheKey = $"rectangles_{a}_{b}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ETLData> result))
            {
                result = await _context.ETLDatas 
                    .ToListAsync();

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
            }

            return result;
        }

        public async Task<IEnumerable<ETLData>> GenerateRectanglesAsync()
        {
            List<ETLData> result = new List<ETLData>();
            Random random = new Random();
            const int count = 25;

            for (int i = 0; i < count; i++)
            {
                result.Add(new ETLData()
                {
                    //Height = random.Next(10),
                    //Width = random.Next(10),
                }
                );
            }
            //await _context.Rectangles.AddRangeAsync(result);
            await _context.SaveChangesAsync();

            // Batch insertion and transaction
            //using (var transaction = await _context.Database.BeginTransactionAsync())
            //{
            //    try
            //    {
            //        await _context.Rectangles.AddRangeAsync(result);
            //        await _context.SaveChangesAsync();
            //        await transaction.CommitAsync();
            //    }
            //    catch (Exception)
            //    {
            //        await transaction.RollbackAsync();
            //        throw;
            //    }
            //}

            return result;
        }
    }
}
