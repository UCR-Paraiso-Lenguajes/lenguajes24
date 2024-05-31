using Microsoft.Extensions.Configuration;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApiTests
{
    public class HappyPathTest
    {
        private IConfiguration _configuration;
        private IProductRepository _productRepository;
        private ISalesRepository _salesRepository;
        [OneTimeSetUp]
        public async Task Setup()
        {
            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();
            _productRepository = new ProductRepository(_configuration);
            _salesRepository = new SalesRepository(_configuration);
        }

        [Test]
        public async Task AddSalesAsync()
        {
            var sales = new Sales
            {
                Date = DateTime.Now,
                Confirmation = 0,
                PaymentMethod = "CASH",
                Total = 100000,
                Address = "San Jose",
                PurchaseNumber = "123456"
            };
            var result = await _salesRepository.AddSalesAsync(sales);
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetSalesByPurchaseNumberAsync()
        {
            var existingPurchaseNumber = "123456";
            var result = await _salesRepository.GetSalesByPurchaseNumberAsync(existingPurchaseNumber);
            Assert.NotNull(result);
            Assert.That(result.PurchaseNumber, Is.EqualTo(existingPurchaseNumber));
        }
    }
}