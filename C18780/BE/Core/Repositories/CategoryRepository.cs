using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;

        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Category> AddCategoryAsync(Category category)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var result = dbContext.Category.Add(category);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<int> DeleteCategoryAsync(Guid uuid)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var filteredData = dbContext.Category.Where(x => x.Uuid == uuid).FirstOrDefault();
                dbContext.Category.Remove(filteredData);
                return await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategoryListAsync()
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Category.ToListAsync();

            }
        }

        public async Task<Category> GetCategoryByIdAsync(Guid uuid)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Category.Where(x => x.Uuid == uuid).FirstOrDefaultAsync();
            }
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                return await dbContext.Category.Where(x => x.Name == name).FirstOrDefaultAsync();
            }
        }
    }
}