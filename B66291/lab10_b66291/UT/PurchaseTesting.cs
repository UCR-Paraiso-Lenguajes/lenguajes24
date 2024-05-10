﻿using NUnit.Framework;
using core;
using System;
using System.Threading.Tasks;
using core.DataBase;
using core.Models;
using core.Business;
namespace UT;

public class PurchaseTesting
{ 

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
        Assert.AreEqual(2, sale.Products.Count());
        Assert.AreEqual("Cachi, Cartago", sale.Address); 
        Assert.Greater(sale.Amount, 0); 
        Assert.AreEqual(PaymentMethods.Type.CASH, sale.PaymentMethod);
        Assert.IsFalse(string.IsNullOrWhiteSpace(sale.PurchaseNumber)); 
        Assert.AreEqual(cart.ProductIds.Count, sale.Products.Count());
    }

    [Test]
    public void Purchase_CartIncompleto()
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
}
