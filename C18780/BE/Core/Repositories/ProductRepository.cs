using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var result = dbContext.Product.Add(product);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<int> DeleteProductAsync(Guid uuid)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var filteredData = dbContext.Product.Where(x => x.Uuid == uuid).FirstOrDefault();
                dbContext.Product.Remove(filteredData);
                return await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetProductByCategoryAsync(Guid category)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Product.Where(x => x.Category == category).ToListAsync();
            }
        }

        public async Task<Product> GetProductByIdAsync(Guid uuid)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Product.Where(x => x.Uuid == uuid).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Product>> GetProductListAsync()
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Product.ToListAsync();
            }
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                dbContext.Product.Update(product);
                return await dbContext.SaveChangesAsync();
            }
        }
    }
}
