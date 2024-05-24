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
        public void TransformarDatos_CasoExitoso()
        {
            List<Report> sales = new List<Report>
            {
                new Report("1", DateTime.Now, 100.50m, 5),
                new Report("2", DateTime.Now, 75.25m, 3)
            };

            var result = ReportLogic.TransformarDatos(sales);
            Assert.AreEqual(2, result.Count());
        }
    

        [Test]
        public void TransformarDatos_ArgumentosNulos()
        {
            List<Report> sales = null;
            Assert.Throws<ArgumentNullException>(() => ReportLogic.TransformarDatos(sales));
        }
    }
}