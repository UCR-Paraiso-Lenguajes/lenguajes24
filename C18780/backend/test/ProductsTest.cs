using StoreApi.Models;
using StoreApi.Repositories;
using Microsoft.Extensions.Configuration;

namespace StoreApiTests
{
    public class ProductsTest
    {
        private IConfiguration _configuration;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;

        [SetUp]
        public async Task SetupAsync()
        {
            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettingstest.json")
                       .Build();
            _productRepository = new ProductRepository(_configuration);
            _categoryRepository = new CategoryRepository(_configuration);

            // Agregar datos de prueba en la tabla Product
            await AddTestProductData();

            // Agregar datos de prueba en la tabla Category
            await AddTestCategoryData();
        }

        private async Task AddTestProductData()
        {
            // Agregar productos de prueba
            var product1 = new Product { Uuid = Guid.Parse("1547f3c3-54e6-4e7d-bf8f-f26daa15c843"), Name = "Producto 1", Category = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18"), Description = "description", ImageUrl = "example", Price = 1 };

            await _productRepository.AddProductAsync(product1);
        }

        private async Task AddTestCategoryData()
        {
            // Agregar categorías de prueba
            var category1 = new Category { Name = "Categoría 1", Uuid = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18") };

            await _categoryRepository.AddCategoryAsync(category1);
        }
    }
}