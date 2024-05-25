using ApiLab7;

namespace UT;

public class PurchaseTests
{
    Store store;

    [OneTimeSetUp]
    public void SetUp()
    {
        Db.BuildDb("Data Source=163.178.173.130;User ID=basesdedatos;Password=BaSesrp.2024; Encrypt=False;");
        store = Store.Instance;
    }

    [Test]
    public void CartThatHasNoProducts_ThrowsArgumentException()
    {
        CartBusiness cartBusiness = new CartBusiness();
        Cart cart = new Cart()
        {
            ProductIds = new List<string>(),
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
        Cart cart = new Cart()
        {
            ProductIds = new List<string> { "11", "22", "33" },
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
        Cart cart = new Cart()
        {
            ProductIds = new List<string> { "11", "22", "33" },
            Address = "A valid address",
            ConfirmationNumber = ""
        };
        Assert.ThrowsAsync<ArgumentException>(() => cartBusiness.PurchaseAsync(cart));
    }

    [Test]
    public void CartThatHasValidArguments_DoesNotThrowsArgumentException()
    {
        CartBusiness cartBusiness = new CartBusiness();
        List<string> products =
        [
            store.ProductsInStore.ElementAt(0).Uuid.ToString(),
            store.ProductsInStore.ElementAt(1).Uuid.ToString(),
            store.ProductsInStore.ElementAt(2).Uuid.ToString()
        ];

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
