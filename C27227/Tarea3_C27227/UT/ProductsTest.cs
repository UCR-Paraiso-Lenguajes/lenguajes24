using Core;
using KEStoreApi;
using KEStoreApi.Data;

namespace UT;

public class ProductsTest
{

    private Products products;
    [SetUp]
    public async Task Setup()
    {
        var dbTest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
        DatabaseConfiguration.Init(dbTest);
        products = await Products.InitializeAsync();
    }

    [Test]
    public async Task GetInstanceAsync_WithValidData_NotReturnsProductsInstance()
    {
        Assert.IsNotNull(products);
    }

    [Test]
    public async Task GetProductsByCategory_ReturnsProductsForValidCategoryId()
    {
        var categoryId = 8;

        var products = await Products.GetProductsByCategory(categoryId);

        Assert.IsNotNull(products);
        Assert.IsInstanceOf<List<Product>>(products);
        Assert.AreEqual(8, products.Count);
    }
    [Test]
    public async Task GetProductsByCategory_ThrowsException_ForInvalidCategoryId()
    {
        int invalidCategoryId = -1;

        Assert.ThrowsAsync<ArgumentException>(async () => await Products.GetProductsByCategory(invalidCategoryId));
    }
}
