using NUnit.Framework;
using core;
using System;
using System.Threading.Tasks;
using core.DataBase;
using core.Models;
using MySqlConnector;

namespace UT
{
    public class ReportDBTesting //falla geekStoreDB.sales
    {
        public class ReportDbTests
        {
            private ReportDb _reportDb;

            [SetUp]
            public void Setup()
            {
                _reportDb = new ReportDb();
            }

            public static async Task CrearDatosAsyncSales()
            {
                using (var connection = new MySqlConnection(Storage.Instance.ConnectionStringMyDb))
                {
                    await connection.OpenAsync();

                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            string createTableQuery = @"
                    DROP TABLE IF EXISTS sales;
                    CREATE TABLE IF NOT EXISTS sales (
                        purchase_number VARCHAR(30) NOT NULL PRIMARY KEY,
                        purchase_date DATETIME NOT NULL,
                        total DECIMAL(10, 2) NOT NULL,
                        payment_type INT,
                        FOREIGN KEY (payment_type) REFERENCES paymentMethod(id)
                    );";

                            using (var command = new MySqlCommand(createTableQuery, connection, transaction))
                            {
                                await command.ExecuteNonQueryAsync();
                            }

                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }

            [Test]
            public async Task ExtraerVentasDiarias_EscenarioExitoso()
            {
                await CrearDatosAsyncSales();
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
                await CrearDatosAsyncSales();
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
                await CrearDatosAsyncSales();
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
                await CrearDatosAsyncSales();
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
                await CrearDatosAsyncSales();
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
