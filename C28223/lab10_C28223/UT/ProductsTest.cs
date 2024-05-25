namespace UT;
using Core;
using storeApi.Models.Data;

public class ProductsTest
{
    private Products productsInstance;

    [SetUp]
    public async Task Setup()
    {
        var dbtestDefault = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
        var myDbtest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
        Storage.Init(dbtestDefault, myDbtest);
        productsInstance = await new Products().GetInstanceAsync();
    }

    [Test]
    public async Task GetInstanceAsync_WithValidData_NotReturnsProductsInstance()// No se crea la instancia hasta que se crea la instancia
    {
        Assert.IsNotNull(productsInstance);
    }


    [Test]
    public async Task GetAllProducts_ReturnsAllProducts()
    {
        var allProducts = productsInstance.GetAllProducts();
        Assert.IsNotNull(allProducts);
        Assert.AreEqual(12, allProducts.Count());// hay 12 pruductos cargados desde la base de datos inicialmente
    }
    [Test]
    public async Task GetProductsBycategoryID_WithValidCategoryID_ReturnsProducts()
    {
        var products = productsInstance.GetProductsBycategoryID(4); // hay 3 productos con categoria 4
        Assert.IsNotNull(products);
        Assert.AreEqual(3, products.Count());
    }

    [Test]
    public async Task GetProductsBycategoryID_WithValidCategoryID_NotReturnsProducts()
    {
        var products = productsInstance.GetProductsBycategoryID(2); // hay 0 productos con categoria 2
        Assert.IsNotNull(products);
        Assert.AreEqual(0, products.Count());
    }

    [Test]
    public async Task GetProductsBycategoryID_WithInvalidCategoryID_ReturnsEmpty()
    {
        var products = productsInstance.GetProductsBycategoryID(999); // ID de categoría inválido
        Assert.IsEmpty(products);
    }

    [Test]
    public async Task GetProductsBycategoryID_WithInvalidNegativeCategoryID_ReturnsThrows()
    {
        Assert.Throws<ArgumentException>(() => productsInstance.GetProductsBycategoryID(-111));
    }
}
