using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using TodoApi;
using TodoApi.Business;
using TodoApi.Database;
using TodoApi.Models;
namespace UT;

public class StoreLogicTest
{
    [SetUp]
    public void Setup()
    {
        Storage.Init("Server=localhost;Port=3407;Database=mysql;Uid=root;Pwd=123456;");
        StoreDB.CreateMysql();
    }

    [Test]
    public async Task Validate_Products_Empty()
    {
        StoreLogic storeLogic = new StoreLogic();
        var list = new List<string> ();
        Cart cart = new Cart(list, "Santiago", 0, 10.0m);
        Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

     [Test]
    public async Task Validate_Address_Empty()
    {
        StoreLogic storeLogic = new StoreLogic();
        var list = new List<string> { "1" };
        Cart cart = new Cart(list, "", 0, 10.0m);
        Assert.ThrowsAsync<ArgumentException>(async () => await storeLogic.PurchaseAsync(cart));
    }

    [Test]
    public async Task HappyPath()
    {
        Categories category = new Categories();
        StoreLogic storeLogic = new StoreLogic();
        var list = new List<String>();
        Product product = new Product("Olla",
         "https://images-na.ssl-images-amazon.com/images/I/71JSM9i1bQL.AC_UL160_SR160,160.jpg",
          45.2m, "Descripción", 1, category.GetType(1));
        list.Add(product.Id.ToString());
        Cart cart = new Cart(list, "Santiago", 0, product.Price);
        var sale = await storeLogic.PurchaseAsync(cart);
        var listProducts = sale.Products;
        Assert.IsNotNull(sale.PurchaseNumber);
        Assert.AreEqual(cart.ProductIds.Count, listProducts.Count());
        Assert.AreEqual(cart.Total, sale.Amount);
    }
}