using NUnit.Framework;
using Store_API.Business;
using Store_API.Database;
using Store_API.Models;


namespace UnitTests
{
    public class SaleReportLogicTests
    {
        private SaleReportLogic _saleReportLogic;
        private readonly DB_API _mockDBApi = new DB_API(); 

        [SetUp]
        public void Setup()
        {
            _saleReportLogic = new SaleReportLogic(_mockDBApi);
        }

        [Test]
        public async Task GenerateSalesReportAsync_ReturnsValidSalesReport()
        {
            DateTime date = new DateTime(2024, 4, 20);

            var mockDailySales = new List<SaleAttribute>
            {
                new SaleAttribute { SaleId = 1, PurchaseNumber = "AFG123", Total = 50.00m, PurchaseDate = date, Product = "Product A", DailySale = "Monday", SaleCounter = 1 },
                new SaleAttribute { SaleId = 2, PurchaseNumber = "BNM456", Total = 35.75m, PurchaseDate = date, Product = "Product B", DailySale = "Monday", SaleCounter = 1 }
            };

            var mockWeeklySales = new List<SaleAttribute>
            {
                new SaleAttribute { SaleId = 3, PurchaseNumber = "CWQ789", Total = 100.00m, PurchaseDate = date, Product = "Product C", DailySale = "Monday", SaleCounter = 1 },
                new SaleAttribute { SaleId = 4, PurchaseNumber = "DYZ012", Total = 75.25m, PurchaseDate = date, Product = "Product D", DailySale = "Monday", SaleCounter = 1 }
            };

            _mockDBApi.ObtainDailySalesAsync = async (_) =>
            {
                await Task.Delay(10); 
                return mockDailySales;
            };

            _mockDBApi.ObtainWeeklySalesAsync = async (_) =>
            {
                await Task.Delay(10); 
                return mockWeeklySales;
            };

            var result = await _saleReportLogic.GenerateSalesReportAsync(date);

            Assert.IsNotNull(result);
            Assert.AreEqual(date, result.Date);
            Assert.IsNotNull(result.DailySales);
            Assert.IsNotNull(result.WeeklySales);
            Assert.IsTrue(result.DailySales.Any(), "No hay ventas diarias en el informe.");
            Assert.IsTrue(result.WeeklySales.Any(), "No hay ventas semanales en el informe.");
            Assert.AreEqual(2, result.CountDailySales(), "El recuento de ventas diarias no coincide.");
            Assert.AreEqual(2, result.CountWeeklySales(), "El recuento de ventas semanales no coincide.");
        }

        [Test]
        public void GenerateSalesReportAsync_WithInvalidDate_ThrowsArgumentException()
        {
            DateTime invalidDate = DateTime.MaxValue;

            Assert.ThrowsAsync<ArgumentException>(() => _saleReportLogic.GenerateSalesReportAsync(invalidDate));
        }

        [Test]
        public void GenerateSalesReportAsync_WithFutureDate_ThrowsArgumentOutOfRangeException()
        {
            DateTime futureDate = DateTime.Now.AddDays(1);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _saleReportLogic.GenerateSalesReportAsync(futureDate));
        }
    }
}