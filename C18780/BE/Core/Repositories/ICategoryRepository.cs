using System.Collections;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetCategoryListAsync();
        public Task<Category> GetCategoryByIdAsync(Guid uuid);
        public Task<Category> GetCategoryByNameAsync(string name);
        public Task<Category> AddCategoryAsync(Category category);
        public Task<int> DeleteCategoryAsync(Guid uuid);
    }
}