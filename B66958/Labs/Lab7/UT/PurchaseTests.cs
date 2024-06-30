using ApiLab7;

namespace UT;

public class PurchaseTests
{
    Store store;

    [OneTimeSetUp]
    public void SetUp()
    {
        Db.BuildDb(
            "Data Source=163.178.173.130;User ID=basesdedatos;Password=BaSesrp.2024; Encrypt=False;"
        );
        store = Store.Instance;
    }

    [Test]
    public void CartThatHasNoProducts_ThrowsArgumentException()
    {
        CartBusiness cartBusiness = new CartBusiness();
        Cart cart = new Cart()
        {
            ProductIds = new List<CartProduct>(),
            Address = "",
            PaymentMethod = 0,
            ConfirmationNumber = ""
        };
        Assert.ThrowsAsync<ArgumentException>(() => cartBusiness.PurchaseAsync(cart));
    }

    [Test]
    public void CartThatHasNoAddress_ThrowsArgumentException()
    {
        CartBusiness cartBusiness = new CartBusiness();
        var cartProduct = new CartProduct
        {
            Id = Guid.NewGuid(),
            Price = 0,
            Quantity = 1
        };
        Cart cart = new Cart()
        {
            ProductIds = new List<CartProduct> { cartProduct, cartProduct, cartProduct },
            Address = "",
            PaymentMethod = 0,
            ConfirmationNumber = ""
        };
        Assert.ThrowsAsync<ArgumentException>(() => cartBusiness.PurchaseAsync(cart));
    }

    [Test]
    public void CartThatHasNoPaymentMethod_ThrowsArgumentException()
    {
        CartBusiness cartBusiness = new CartBusiness();
        var cartProduct = new CartProduct
        {
            Id = Guid.NewGuid(),
            Price = 0,
            Quantity = 1
        };
        Cart cart = new Cart()
        {
            ProductIds = new List<CartProduct> { cartProduct, cartProduct, cartProduct },
            Address = "A valid address",
            ConfirmationNumber = ""
        };
        Assert.ThrowsAsync<ArgumentException>(() => cartBusiness.PurchaseAsync(cart));
    }

    [Test]
    public void CartThatHasValidArguments_DoesNotThrowsArgumentException()
    {
        CartBusiness cartBusiness = new CartBusiness();
        var cartProduct = new CartProduct
        {
            Id = store.ProductsInStore.ElementAt(0).Uuid,
            Price = 0,
            Quantity = 1
        };
        var cartProduct2 = new CartProduct
        {
            Id = store.ProductsInStore.ElementAt(1).Uuid,
            Price = 0,
            Quantity = 1
        };
        var cartProduct3 = new CartProduct
        {
            Id = store.ProductsInStore.ElementAt(2).Uuid,
            Price = 0,
            Quantity = 1
        };
        List<CartProduct> products = [cartProduct, cartProduct2, cartProduct3];

        Cart cart = new Cart()
        {
            ProductIds = products,
            Address = "A valid address",
            PaymentMethod = 0,
            ConfirmationNumber = ""
        };
        Assert.DoesNotThrowAsync(() => cartBusiness.PurchaseAsync(cart));
    }
}
