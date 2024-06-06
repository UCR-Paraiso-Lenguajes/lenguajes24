using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly IConfiguration _configuration;
        public SalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Sales> AddSalesAsync(Sales sales)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var result = dbContext.Sales.Add(sales);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<Sales> GetSalesByIdAsync(Guid uuid)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Sales.Where(x => x.Uuid == uuid).FirstOrDefaultAsync();
            }
        }

        public async Task<Sales> GetSalesByPurchaseNumberAsync(string purchaseNumber)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Sales.Where(x => x.PurchaseNumber == purchaseNumber).FirstOrDefaultAsync();
            }
        }

        public async Task<int> UpdateSalesAsync(Sales sales)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                dbContext.Sales.Update(sales);
                return await dbContext.SaveChangesAsync();
            }
        }
    }
}