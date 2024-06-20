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
        Storage.Init("Server=localhost;Port=3407;Database=mysql;Uid=root;Pwd=123456;");
        StoreDB.CreateMysql();
    }

    [Test]
    public async Task Validate_Date_IsMinValue()
    {
        var saleDB = new SaleDB();
        Assert.ThrowsAsync<ArgumentException>(async () => saleDB.GetSalesReport(DateTime.MinValue));
    }

    [Test]
    public async Task Validate_Return_SalesReport()
    {
        var saleDB = new SaleDB();
        DateTime validDate = new DateTime(2024, 5, 3);

        var result = saleDB.GetSalesReport(validDate);

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