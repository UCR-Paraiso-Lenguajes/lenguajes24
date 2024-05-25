using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetProductListAsync();
        public Task<Product> GetProductByIdAsync(Guid uuid);
        public Task<List<Product>> GetProductByCategoryAsync(Guid category);
        public Task<Product> AddProductAsync(Product product);
        public Task<int> UpdateProductAsync(Product product);
        public Task<int> DeleteProductAsync(Guid uuid);
    }
}
