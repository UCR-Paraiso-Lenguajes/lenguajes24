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
        private ReportDb _reportDb;

        [SetUp]
        public void Setup()
        {
            var myDbtest = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            myDbtest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            StoreDb.CrearDatosSync();
            _reportDb = new ReportDb();
        }

        [Test]
        public async Task ExtraerVentasDiarias_EscenarioExitoso()
        {
            DateTime date = DateTime.Today;
            IEnumerable<Report> salesList;
            salesList = await ReportDb.ExtraerVentasDiariasAsync(date);
            Assert.IsNotNull(salesList);
        }

        [Test]
        public async Task ExtraerVentasDiarias_ArgumentoVacio()
        {
            DateTime date = DateTime.Today;
            IEnumerable<Report> salesList;
            salesList = await ReportDb.ExtraerVentasDiariasAsync(date);
            Assert.IsNotNull(salesList);
        }


        [Test]
        public async Task ExtraerVentasSemanal_EscenarioExitoso()
        {
            DateTime selectedDate = DateTime.Today;
            IEnumerable<Report> salesList;
            salesList = await ReportDb.ExtraerVentasSemanalAsync(selectedDate);
            Assert.IsNotNull(salesList);
        }

        [Test]
        public async Task TestExtraerVentasSemanal_VentaSemanalExistent()
        {
            DateTime selectedDate = new DateTime(2024, 5, 6);
            ReportDb reportDb = new ReportDb();
            IEnumerable<Report> weeklySales = await ReportDb.ExtraerVentasSemanalAsync(selectedDate);
            Assert.IsNotNull(weeklySales);
            Assert.False(weeklySales.Count() > 0);
        }

    }
}
