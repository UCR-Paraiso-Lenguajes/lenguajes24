using Core;
using NUnit.Framework;
using storeApi;
using storeApi.Business;
using storeApi.Database;
using storeApi.Models;
namespace UT;

public class StoreLogicTest
{

    [SetUp]
    public void Setup()
    {
        ConnectionDB.Init("Server=localhost;Port=3407;Database=store;Uid=root;Pwd=123456;");
        StoreLogic _storeLogic = new StoreLogic(); // Asegúrate de tener una clase StoreLogic para probar

    }

    [Test]
    public async Task PurchaseAsync_WithEmptyCart_ShouldThrowArgumentException()
    {
        var cart = new Cart { ProductIds = new List<string>(), Address = "Test Address" };
        StoreLogic storeLogic = new StoreLogic();

        Assert.ThrowsAsync<ArgumentException>(() => storeLogic.PurchaseAsync(cart));
    }

    [Test]
    public async Task PurchaseAsync_WithNoAddress_ShouldThrowArgumentException()
    {
        StoreLogic storeLogic = new StoreLogic();
        var cart = new Cart { ProductIds = new List<string> { "1", "2", "3" }, Address = "" };

        ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(() => storeLogic.PurchaseAsync(cart));
        Assert.That(exception.Message, Is.EqualTo("Address must be provided."));
    }

}