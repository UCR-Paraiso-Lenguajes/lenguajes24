/* using NUnit.Framework;
using System;

[TestFixture]
public class CartTests
{
    [Test]
    public void Cart_WithNoProducts_ThrowsArgumentException()
    {
        // Arrange
        var cart = new Cart
        {
            ProductIds = new List<int>(),
            Address = "123 Main St"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Store().Purchase(cart));
    }

    [Test]
    public void Cart_WithNoAddress_ThrowsArgumentException()
    {
        // Arrange
        var cart = new Cart
        {
            ProductIds = new List<int> { 1, 2, 3 },
            Address = ""
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Store().Purchase(cart));
    }

    [Test]
    public void Cart_WithValidProductsAndAddress_DoesNotThrowException()
    {
        // Arrange
        var cart = new Cart
        {
            ProductIds = new List<int> { 1, 2, 3 },
            Address = "123 Main St"
        };

        // Act & Assert
        Assert.DoesNotThrow(() => new Store().Purchase(cart));
    }
}
 */