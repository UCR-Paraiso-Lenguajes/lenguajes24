using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace storeapi.UT
{
    [TestFixture]
    public class SalesDBTests
    {
        [Test]
        public async Task GetForWeekAsync_ReturnsMockDataWithTotal()
        {
            // Arrange
            var salesDB = new MockSalesDB();

            // Act
            var result = await salesDB.GetForWeekAsync("2024-04-30");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // Verifica la cantidad de filas esperada

            // Validar el total de las transacciones semanales
            decimal totalWeek = result.Sum(row => decimal.Parse(row[0])); // Suma de los totales en la primera columna
            Assert.AreEqual(250M, totalWeek); // Verifica que el total sea el esperado
        }

        [Test]
        public async Task GetForDayAsync_ReturnsMockDataWithTotal()
        {
            // Arrange
            var salesDB = new MockSalesDB();

            // Act
            var result = await salesDB.GetForDayAsync("2024-04-30");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count()); // Verifica la cantidad de filas esperada

            // Validar el total de las transacciones diarias
            decimal totalDay = result.Sum(row => decimal.Parse(row[0])); // Suma de los totales en la primera columna
            Assert.AreEqual(200M, totalDay); // Verifica que el total sea el esperado
        }
    }
}
