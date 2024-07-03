using StoreApi.Models;
using StoreApi.Repositories;
using Microsoft.Extensions.Configuration;

namespace StoreApiTests
{
    public class HappyPathTest
    {
        private IConfiguration _configuration;
        private IProductRepository _productRepository;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();
            _productRepository = new ProductRepository(_configuration);
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