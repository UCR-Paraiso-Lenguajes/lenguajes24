using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using StoreApi.Repositories;
using StoreApi.Models;

namespace StoreApiTests
{
    public class DailySalesRepositoryTests
    {
        private IConfiguration _configuration;
        private IDailySalesRepository _dailySalesRepository;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                          .AddJsonFile("appsettings.json")
                          .Build();

            _dailySalesRepository = new DailySalesRepository(_configuration);
        }

        [Test, Order(1)]
        public async Task GetDailySalesListAsync_ValidDate_ReturnsDailySalesList()
        {
            // Preparar datos de prueba
            var date = new DateTime(2024, 6, 30);
            var sales = new List<Sale>
            {
                new Sale { Uuid = Guid.NewGuid(), Date = date, PaymentMethod = "Cash" },
                new Sale { Uuid = Guid.NewGuid(), Date = date, PaymentMethod = "Card" }
            };
            await _dbContext.Sales.AddRangeAsync(sales);
            await _dbContext.SaveChangesAsync();

            var salesLines = new List<SalesLine>
            {
                new SalesLine { Uuid = Guid.NewGuid(), UuidSales = sales[0].Uuid, UuidProduct = Guid.NewGuid(), Quantity = 2, Subtotal = 100 },
                new SalesLine { Uuid = Guid.NewGuid(), UuidSales = sales[1].Uuid, UuidProduct = Guid.NewGuid(), Quantity = 1, Subtotal = 50 }
            };
            await _dbContext.SalesLine.AddRangeAsync(salesLines);
            await _dbContext.SaveChangesAsync();

            var products = new List<Product>
            {
                new Product { Uuid = salesLines[0].UuidProduct, Name = "Product 1" },
                new Product { Uuid = salesLines[1].UuidProduct, Name = "Product 2" }
            };
            await _dbContext.Product.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();

            // Ejecutar el método bajo prueba
            var result = await _dailySalesRepository.GetDailySalesListAsync(date);

            // Afirmar resultados
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());

            var dailySales1 = result.FirstOrDefault(ds => ds.PaymentMethod == "Cash");
            Assert.NotNull(dailySales1);
            Assert.AreEqual(200, dailySales1.Total); // 2 * 100

            var dailySales2 = result.FirstOrDefault(ds => ds.PaymentMethod == "Card");
            Assert.NotNull(dailySales2);
            Assert.AreEqual(50, dailySales2.Total); // 1 * 50
        }
    }
}
