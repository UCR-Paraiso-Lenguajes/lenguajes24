using System.Security.Cryptography.X509Certificates;
using TodoApi;
using TodoApi.Business;
using TodoApi.Database;
using TodoApi.Models;
namespace UT;
public class SaleDBTest
{

    [SetUp]
    public void Setup()
    {
        Storage.Init("Server=localhost;Port=3407;Database=store;Uid=root;Pwd=123456;");
    }

    [Test]
    public async Task Validate_Date_IsMinValue()
    {
        var saleDB = new SaleDB();
        Assert.ThrowsAsync<ArgumentException>(async () => await saleDB.GetSalesReportAsync(DateTime.MinValue));
    }

    [Test]
    public async Task Validate_Return_SalesReport()
    {
        var saleDB = new SaleDB();
        DateTime validDate = new DateTime(2024, 5, 3);

        var result = await saleDB.GetSalesReportAsync(validDate);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<SalesReport>(result);
    }

    [Test]
    public void Validate_Sale_IsNull()
    {
        var saleDB = new SaleDB();
        Assert.ThrowsAsync<ArgumentException>(async () => await saleDB.SaveAsync(null));
    }

}