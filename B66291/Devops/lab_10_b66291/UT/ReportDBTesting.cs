using NUnit.Framework;
using core;
using System;
using System.Threading.Tasks;
using core.DataBase;
using core.Models;

namespace UT
{
    public class ReportDBTesting
    {
        [TestFixture]
        public class ReportDbTests
        {
            private ReportDb _reportDb;

            [SetUp]
            public async void Setup()
            {
                _reportDb = new ReportDb();
                await StoreDb.CrearDatosAsync();
            }

            [Test]
            public async Task ExtraerVentasDiarias_EscenarioExitoso()
            {
                DateTime date = DateTime.Today;

                IEnumerable<Report> salesList;

                var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
                Storage.Init(myDbtest);

                salesList = await ReportDb.ExtraerVentasDiariasAsync(date);

                Assert.IsNotNull(salesList);
            }

            [Test]
            public async Task TestExtraerVentasDiarias_ReturnsValidData()
            {
                DateTime date = DateTime.Today;

                ReportDb reportDb = new ReportDb();

                var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
                Storage.Init(myDbtest);

                IEnumerable<Report> dailySales = await ReportDb.ExtraerVentasDiariasAsync(date);

                Assert.IsNotNull(dailySales);
                Assert.IsTrue(dailySales.Count() > 0);
            }

            [Test]
            public async Task ExtraerVentasDiarias_ArgumentoVacio()
            {
                DateTime date = DateTime.Today;
                IEnumerable<Report> salesList;
                var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
                Storage.Init(myDbtest);
                salesList = await ReportDb.ExtraerVentasDiariasAsync(date);
                Assert.IsNotNull(salesList);
            }


            [Test]
            public async Task ExtraerVentasSemanal_EscenarioExitoso()
            {
                DateTime selectedDate = DateTime.Today;
                IEnumerable<Report> salesList;
                var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
                Storage.Init(myDbtest);
                salesList = await ReportDb.ExtraerVentasSemanalAsync(selectedDate);
                Assert.IsNotNull(salesList);
            }

            [Test]
            public async Task TestExtraerVentasSemanal_VentaSemanalExistent()
            {
                DateTime selectedDate = new DateTime(2024, 5, 6);
                ReportDb reportDb = new ReportDb();
                var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
                Storage.Init(myDbtest);
                IEnumerable<Report> weeklySales = await ReportDb.ExtraerVentasSemanalAsync(selectedDate);
                Assert.IsNotNull(weeklySales);
                Assert.False(weeklySales.Count() > 0);
            }

        }

    }
}
