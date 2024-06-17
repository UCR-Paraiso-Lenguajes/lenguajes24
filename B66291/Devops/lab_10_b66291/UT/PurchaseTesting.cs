using NUnit.Framework;
using core;
using System;
using System.Threading.Tasks;
using core.DataBase;
using core.Models;
using core.Business;
namespace UT;

public class PurchaseTesting
{
    private PurchaseTesting _purchaseTest;

    [SetUp]
    public void Setup()
    {
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init(myDbtest);
        _purchaseTest = new PurchaseTesting();
        StoreDb.CrearDatosSync();
    }

    [Test]
    public async Task PurchaseTest_ExcenarioExitoso()
    {
        var cart = new Cart
        {
            ProductIds = new List<string> { "1", "2" },
            Address = "Cachi, Cartago",
            PaymentMethod = PaymentMethods.Type.CASH
        };

        var saleInfo = new StoreLogic();

        var sale = await saleInfo.PurchaseAsync(cart);

        Assert.IsNotNull(sale);
        Assert.That(sale.Products.Count(), Is.EqualTo(2));
        Assert.That(sale.Address, Is.EqualTo("Cachi, Cartago"));
        Assert.Greater(sale.Amount, 0);
        Assert.That(sale.PaymentMethod, Is.EqualTo(PaymentMethods.Type.CASH));
        Assert.IsFalse(string.IsNullOrWhiteSpace(sale.PurchaseNumber));
    }

    [Test]
    public void PurchaseTest_CarritoIncompleto()
    {
        var cart = new Cart
        {
            ProductIds = new List<string> { "1", "2" },
            Address = "",
            PaymentMethod = PaymentMethods.Type.CASH,
        };
        var purchaseService = new StoreLogic();
        Assert.ThrowsAsync<ArgumentException>(async () => await purchaseService.PurchaseAsync(cart));
    }

    [Test]
    public void PurchaseTest_CarritoVacio()
    {
        var cart = new Cart
        {
            ProductIds = new List<string>(),
            Address = "Cachi, Cartago",
            PaymentMethod = PaymentMethods.Type.CASH,
        };
        var purchaseService = new StoreLogic();
        Assert.ThrowsAsync<ArgumentException>(async () => await purchaseService.PurchaseAsync(cart));
    }

}