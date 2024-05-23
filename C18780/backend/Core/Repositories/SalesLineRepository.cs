using Microsoft.Extensions.Configuration;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class SalesLineRepository : ISalesLineRepository
    {
        private readonly IConfiguration _configuration;
        public SalesLineRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<SalesLine> AddSalesLineAsync(SalesLine salesLine)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var result = dbContext.SalesLine.Add(salesLine);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }
    }
}