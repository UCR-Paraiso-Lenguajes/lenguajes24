using Core;
using storeApi;
using storeApi.Business;
using storeApi.Database;
using storeApi.Models;
namespace UT;
public class SaleDBTest
{

  private SaleDB _saleDB;

        [SetUp]
        public void Setup()
        {
            ConnectionDB.Init("Server=172.20.192.1;Database=mysql;Uid=root;Pwd=123456;");
            _saleDB = new SaleDB();
        }

        [Test]
        public async Task SaveAsync_WithNullSale_ShouldThrowArgumentException()
        {
            // Arrange
            Sale sale = null;

            // Act & Assert
            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(() => _saleDB.SaveAsync(sale));
            StringAssert.Contains("Sale must contain at least one product.", exception.Message);
        }

        [Test]
        public async Task GetWeekSalesAsync_WithMinDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime date = DateTime.MinValue;

            // Act & Assert
            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(() => _saleDB.getWeekSalesAsync(date));
            StringAssert.Contains("Invalid date.", exception.Message);
        }

        [Test]
        public async Task GetDailySales_WithMinDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime date = DateTime.MinValue;

            // Act & Assert
            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(() => _saleDB.getDailySales(date));
            StringAssert.Contains("Invalid date.", exception.Message);
        }

        [Test]
        public async Task GetDailySales_WithNonTodayDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Now.AddDays(-1); // Any non-today date

            // Act & Assert
            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(() => _saleDB.getDailySales(date));
            StringAssert.Contains("Invalid date.", exception.Message);
        }


}