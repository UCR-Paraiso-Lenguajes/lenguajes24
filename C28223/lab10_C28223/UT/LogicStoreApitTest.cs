namespace UT;
using storeApi.Business;
using storeApi.Models;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Core;
using storeApi;

public class LogicStoreApitTest
{

    public void Setup()
    {
        var dbtestDefault = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
        Storage.Init(dbtestDefault);
    }

    [Test]
    public async Task PurchaseAsync_ValidCart_ReturnsSale()// HappyPath 
    {
        // Arrange
        var logicStoreApi = new LogicStoreApi();
        var cart = new Cart
        {
            ProductIds = new List<ProductQuantity>
                {// todos los productos valen 20 000 
                    new ProductQuantity ("1",6),
                    new ProductQuantity ("2",8),
                    new ProductQuantity ("3",1),
                    new ProductQuantity ("4",4)
                },
            Address = "Cartago:CostaRica",
            PaymentMethod = 0
        };
        // Act
        var sale = await logicStoreApi.PurchaseAsync(cart);
        // Assert
        Assert.IsNotNull(sale);
        Assert.IsInstanceOf<Sale>(sale);
        Assert.AreEqual(4, sale.Products.Count()); // Two products in the sale
        Assert.AreEqual(380000.00, sale.Amount); // Total amount  429400
        Assert.AreEqual("Cartago:CostaRica", sale.Address);
        Assert.AreEqual(PaymentMethods.Type.CASH, sale.PaymentMethod);
        Assert.IsFalse(string.IsNullOrEmpty(sale.PurchaseNumber));
    }

    [Test]
    public void PurchaseAsync_EmptyCart_ThrowsArgumentException()
    {
        // Arrange
        var logicStoreApi = new LogicStoreApi();
        var emptyCart = new Cart {};
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await logicStoreApi.PurchaseAsync(emptyCart));
    }
    [Test]
    public async Task PurchaseAsync_ProductIdsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var logicStoreApi = new LogicStoreApi();
        var cart = new Cart
        {
            ProductIds = new List<ProductQuantity>(), // Carrito vacío
            Address = "Cartago:CostaRica",
            PaymentMethod = PaymentMethods.Type.CASH
        };
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await logicStoreApi.PurchaseAsync(cart));
    }
    [Test]
    public async Task PurchaseAsync_AddressNullOrWhiteSpace_ThrowsArgumentException()
    {
        // Arrange
        var logicStoreApi = new LogicStoreApi();
        var cart = new Cart
        {
            ProductIds = new List<ProductQuantity>
        {
            new ProductQuantity("1", 2),
            new ProductQuantity("2", 1)
        },
            Address = "", // Dirección vacía
            PaymentMethod = PaymentMethods.Type.CASH
        };
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await logicStoreApi.PurchaseAsync(cart));
    }

}
