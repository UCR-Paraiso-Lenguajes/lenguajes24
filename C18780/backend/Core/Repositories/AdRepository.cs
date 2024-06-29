using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class AdRepository : IAdRepository
    {

        private readonly IConfiguration _configuration;

        public AdRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Ad> GetAdByIdAsync(Guid uuid)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Ad.Where(x => x.Uuid == uuid).FirstOrDefaultAsync();
            }
        }
        public async Task<Ad> AddAdAsync(Ad ad)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var result = dbContext.Ad.Add(ad);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<int> DeleteAdAsync(Guid uuid)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var filteredData = dbContext.Ad.Where(x => x.Uuid == uuid).FirstOrDefault();
                dbContext.Ad.Remove(filteredData);
                return await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Ad>> GetAdListAsync()
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Ad.ToListAsync();
            }
        }

        public async Task<List<Ad>> GetLatestAdsAsync(int count)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Ad.OrderByDescending(x => x.Date).Take(count).ToListAsync();
            }
        }
    }
}