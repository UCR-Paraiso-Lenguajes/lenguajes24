using ApiLab7;

namespace UT;

public class ProductTests
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
    public void AddingNewProductWithoutName_ThrowsException()
    {
        ProductBusiness productBusiness = new ProductBusiness();
        var product = new ProductDTO
        {
            Name = "",
            ImageUrl = "www.image.com",
            Description = "description",
            Price = 2500,
            Category = 2
        };
        Assert.ThrowsAsync<ArgumentException>(() => productBusiness.AddProduct(product));
    }

    [Test]
    public void AddingNewProductWithoutImageUrl_ThrowsException()
    {
        ProductBusiness productBusiness = new ProductBusiness();
        var product = new ProductDTO
        {
            Name = "product",
            ImageUrl = "",
            Description = "description",
            Price = 2500,
            Category = 2
        };
        Assert.ThrowsAsync<ArgumentException>(() => productBusiness.AddProduct(product));
    }

    [Test]
    public void AddingNewProductWithoutDescription_ThrowsException()
    {
        ProductBusiness productBusiness = new ProductBusiness();
        var product = new ProductDTO
        {
            Name = "product",
            ImageUrl = "www.image.com",
            Description = "",
            Price = 2500,
            Category = 2
        };
        Assert.ThrowsAsync<ArgumentException>(() => productBusiness.AddProduct(product));
    }

    [Test]
    public void AddingNewProductWithoutPriceZero_ThrowsException()
    {
        ProductBusiness productBusiness = new ProductBusiness();
        var product = new ProductDTO
        {
            Name = "product",
            ImageUrl = "www.image.com",
            Description = "desc",
            Price = 0,
            Category = 2
        };
        Assert.ThrowsAsync<ArgumentException>(() => productBusiness.AddProduct(product));
    }

    [Test]
    public void AddingNewProductWithoutCategoryZero_ThrowsException()
    {
        ProductBusiness productBusiness = new ProductBusiness();
        var product = new ProductDTO
        {
            Name = "product",
            ImageUrl = "www.image.com",
            Description = "desc",
            Price = 25000,
            Category = 0
        };
        Assert.ThrowsAsync<ArgumentException>(() => productBusiness.AddProduct(product));
    }

    [Test]
    public void AddingNewProductWithCorrectData_DoesNotThrowException()
    {
        ProductBusiness productBusiness = new ProductBusiness();
        var product = new ProductDTO
        {
            Name = "product",
            ImageUrl = "www.image.com",
            Description = "desc",
            Price = 25000,
            Category = 1
        };
        Assert.DoesNotThrowAsync(() => productBusiness.AddProduct(product));
    }
}