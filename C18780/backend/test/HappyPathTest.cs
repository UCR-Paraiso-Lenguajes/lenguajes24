using Microsoft.Extensions.Configuration;
using Moq;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApiTests
{
    public class HappyPathTest
    {
        private IConfiguration _configuration;
        private ISalesRepository _salesRepository;
        [OneTimeSetUp]
        public async Task Setup()
        {
            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();

            var salesRepositoryMock = new Mock<ISalesRepository>();

            //Configuracion de los metodos simulados de ISalesRepository
            salesRepositoryMock.Setup(repo => repo.AddSalesAsync(It.IsAny<Sales>()))
                                .ReturnsAsync(new Sales() { Uuid = Guid.NewGuid() });

            salesRepositoryMock.Setup(repo => repo.GetSalesByPurchaseNumberAsync(It.IsAny<string>()))
                                .ReturnsAsync(new Sales() { Uuid = Guid.NewGuid() });

            _salesRepository = salesRepositoryMock.Object;
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
    }
}