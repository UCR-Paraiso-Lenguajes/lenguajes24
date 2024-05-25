using Core;
using StoreAPI.Business;
using StoreAPI.Database;
using StoreAPI.models;


namespace UT
{

  public class TestsSaleLogic
  {

    private SaleReportLogic saleReportLogic;

    [SetUp]
    public async Task SetupAsync()
    {
      string connectionString = "Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;";
      Storage.Init(connectionString);
      StoreDB.CreateMysql();
      
      saleReportLogic = new SaleReportLogic();
    }

    [Test]
    public async Task GetSalesReportAsync_ValidDate_ReturnsSalesReport()
    {
      DateTime validDate = new DateTime(2024, 5, 3);

      SalesReport salesReport = await saleReportLogic.GetSalesReportAsync(validDate);

      Assert.IsNotNull(salesReport);
      Assert.IsNotNull(salesReport.DailySales);
      Assert.IsNotNull(salesReport.WeeklySales);
    }

    [Test]
    public void GetSalesReportAsync_InvalidDate_ThrowsArgumentException()
    {
      DateTime invalidDate = DateTime.MinValue;

      Assert.ThrowsAsync<ArgumentException>(async () =>
      {
        await saleReportLogic.GetSalesReportAsync(invalidDate);
      });

    }
    //Happy path
    [Test]
    public async Task GetSalesReportAsync_HappyPath_ReturnsSalesReportWithValidData()
    {
      DateTime validDate = new DateTime(2024, 5, 3);

      SalesReport salesReport = await saleReportLogic.GetSalesReportAsync(validDate);

      Assert.IsNotNull(salesReport);
      Assert.IsNotNull(salesReport.DailySales);
      Assert.IsNotNull(salesReport.WeeklySales);
      Assert.IsFalse(salesReport.DailySales.Any());
      Assert.IsTrue(salesReport.WeeklySales.Any());
    }



  }
}
