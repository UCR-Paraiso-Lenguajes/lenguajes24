using StoreApi.Models;
using StoreApi.Repositories;
using Microsoft.Extensions.Configuration;

namespace StoreApiTests
{
    public class HappyPathTest
    {
        private IConfiguration _configuration;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;

        [SetUp]
        public async Task Setup()
        {
            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
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


        [Test]
        public async Task AddSalesAsync_ValidSales_ReturnsSales()
        {
            var salesRepository = new SalesRepository(_configuration);
            var sales = new Sales
            {
                Date = DateTime.Now,
                Confirmation = 0,
                PaymentMethod = "CASH",
                Total = 100000,
                Address = "San Jose",
                PurchaseNumber = "123456"
            };

            var result = await salesRepository.AddSalesAsync(sales);

            Assert.NotNull(result);
            Assert.AreEqual(sales.Date, result.Date);
            Assert.AreEqual(sales.Confirmation, result.Confirmation);
            Assert.AreEqual(sales.PaymentMethod, result.PaymentMethod);
            Assert.AreEqual(sales.Total, result.Total);
            Assert.AreEqual(sales.Address, result.Address);
            Assert.AreEqual(sales.PurchaseNumber, result.PurchaseNumber);
        }

        [Test]
        public async Task GetSalesByPurchaseNumberAsync_ExistingPurchaseNumber_ReturnsSales()
        {
            var salesRepository = new SalesRepository(_configuration);
            var existingPurchaseNumber = "123456";

            var result = await salesRepository.GetSalesByPurchaseNumberAsync(existingPurchaseNumber);

            Assert.NotNull(result);
            Assert.AreEqual(existingPurchaseNumber, result.PurchaseNumber);
        }

        [Test]
        public async Task GetSalesByPurchaseNumberAsync_NonExistingPurchaseNumber_ReturnsNull()
        {
            var salesRepository = new SalesRepository(_configuration);
            var nonExistingPurchaseNumber = "999999";

            var result = await salesRepository.GetSalesByPurchaseNumberAsync(nonExistingPurchaseNumber);

            Assert.Null(result);
        }
    }
}