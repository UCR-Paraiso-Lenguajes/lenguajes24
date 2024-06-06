using NUnit.Framework;
using core.Business; 
using core.Models;

namespace UT
{
    [TestFixture]
    public class LogicSalesReportsApiTests
    {
        private ReportLogic _reportLogic;

        [SetUp]
        public void Setup()
        {
            _reportLogic = new ReportLogic();
        }

        [Test]
        public void listarReportes_ExcenarioExistoso()
        {
            List<Report> dailySales = new List<Report>
            {
                new { purchaseNumber = "1111", purchaseDate = new DateTime(2023, 5, 1), total = 100.50m },
                new { purchaseNumber = "2222", purchaseDate = new DateTime(2023, 5, 2), total = 75.25m }
            };

            List<Report> weeklySales = new List<Report>
            {
                new { purchaseNumber = "3333", purchaseDate = new DateTime(2023, 4, 28), total = 200.00m },
                new { purchaseNumber = "4444", purchaseDate = new DateTime(2023, 5, 3), total = 150.75m }
            };

            List<Report>[] result = ReportLogic.listarReportes(dailySales, weeklySales);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(dailySales, result[0]); 
            Assert.AreEqual(weeklySales, result[1]); 
        }
    

        [Test]
        public void TransformarDatos_CasoExitoso()
        {
            IEnumerable<Report> sales = new List<Report>
            {
                new Report("1", DateTime.Now, 100.50m, 5),
                new Report("2", DateTime.Now, 75.25m, 3)
            };

            var result = ReportLogic.TransformarDatos(sales);
            Assert.AreEqual(2, result.Count);
        }
    

        [Test]
        public void TransformarDatos_ArgumentosNulos()
        {
            IEnumerable<Report> sales = null;
            Assert.Throws<ArgumentNullException>(() => ReportLogic.TransformarDatos(sales));
        }
    }
}