using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KEStoreApi;
using Core;
using static KEStoreApi.SaleLogic;

namespace UnitTests
{
    public class SaleLogicTests
    {
        private SaleLogic _saleLogic;

        [SetUp]
        public void Setup()
        
        {
             string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(connectionString);

            _saleLogic = new SaleLogic();
        }

        [Test]
        public async Task GetReportSalesAsync_ReturnsValidReportSales()
        {
            // Arrange
            DateTime date = DateTime.Now;

            // Act
            ReportSales result = await _saleLogic.GetReportSalesAsync(date);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Date, Is.EqualTo(date));
            Assert.IsNotNull(result.Sales);
            Assert.IsNotNull(result.SalesByWeek);
            Assert.IsTrue(result.Sales.Any(), "No hay ventas en el informe.");

            // Validar los montos
            decimal totalAmount = result.Sales.Sum(s => s.Total);
            Assert.Greater(totalAmount, 0, "El monto total de ventas debe ser mayor que cero.");

            // Validar la cantidad de filas
            int expectedRowCount = 2; // Ajusta este valor según tus requisitos
            Assert.That(result.Sales.Count(), Is.EqualTo(expectedRowCount), $"La cantidad de filas debe ser {expectedRowCount}.");
        }
        [Test]
        public async Task GetReportSalesAsync_WithInvalidDate_ThrowsException()
        {
            // Arrange
            DateTime invalidDate = DateTime.MaxValue;
                    
            // Act & Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _saleLogic.GetReportSalesAsync(invalidDate));
        }

        [Test]
        public void GetReportSalesAsync_WithNullDate_ThrowsException()
        {
            DateTime minValueDate = DateTime.MinValue;
            Assert.ThrowsAsync<ArgumentException>(() => _saleLogic.GetReportSalesAsync(minValueDate)); 
        }

    }
    
}
