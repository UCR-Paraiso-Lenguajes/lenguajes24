using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KEStoreApi;
using Core;
using static KEStoreApi.SaleLogic;
using KEStoreApi.Data;

namespace UnitTests
{
    public class SaleLogicTests
    {
        private SaleLogic _saleLogic;

        [SetUp]
        public void Setup()
        
        {
             var dbTest = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(dbTest);
            DatabaseStore.Store_MySql();
             string connectionString = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            DatabaseConfiguration.Init(connectionString);

            _saleLogic = new SaleLogic();
        }

        [Test]
        public async Task GetReportSalesAsync_ReturnsValidReportSales()
        {
            DateTime date = new DateTime(2024, 4, 7);


            ReportSales result = await _saleLogic.GetReportSalesAsync(date);

            Assert.IsNotNull(result);
            Assert.That(result.Date, Is.EqualTo(date));
            Assert.IsNotNull(result.Sales);
            Assert.IsNotNull(result.SalesByWeek);
            Assert.IsTrue(result.Sales.Any(), "No hay ventas en el informe.");
            decimal totalAmount = result.Sales.Sum(s => s.Total);
            Assert.Greater(totalAmount, 0, "El monto total de ventas debe ser mayor que cero.");
            int expectedRowCount = 3;
            Assert.That(result.Sales.Count(), Is.EqualTo(expectedRowCount), $"La cantidad de filas debe ser {expectedRowCount}.");
        }
        [Test]
        public async Task GetReportSalesAsync_WithMaxValue_ThrowsException()
        {
            DateTime MaxValueDate = DateTime.MaxValue;
                    

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _saleLogic.GetReportSalesAsync(MaxValueDate));
        }

        [Test]
        public void GetReportSalesAsync_WithMinDate_ThrowsException()
        {
            DateTime minValueDate = DateTime.MinValue;
            Assert.ThrowsAsync<ArgumentException>(() => _saleLogic.GetReportSalesAsync(minValueDate)); 
        }

        [Test]

        public async Task GetReportSalesAsync_NoDataSales(){

            DateTime dateTime = new DateTime(2024, 2, 23);

            ReportSales reportSales = await _saleLogic.GetReportSalesAsync(dateTime);
            Assert.That(reportSales.Sales.Count(), Is.EqualTo(0));
            Assert.That(reportSales.SalesByWeek.Count(), Is.EqualTo(0));
            
        }

    }
    
}
