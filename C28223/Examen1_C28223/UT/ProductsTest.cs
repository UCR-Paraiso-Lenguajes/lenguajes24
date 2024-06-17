namespace UT;
using Core;
using storeApi.Models.Data;
using storeApi.DataBase;


public class ProductsTest
{
    private Products productsInstance;

    [SetUp]
    public async Task Setup()
    {
        var dbtestDefault = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
        Storage.Init(dbtestDefault);
        productsInstance = await new Products().GetInstanceAsync();
        StoreDataBase.CreateMysql();
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
        var categories = new List<int> { 4 }; // Categoría 4 tiene 3 productos
        var products = productsInstance.GetProductsByCategoryIDs(categories);
        Assert.IsNotNull(products);
        Assert.AreEqual(3, products.Count());
    }

    [Test]
    public async Task GetProductsBycategoryID_WithValidCategoryID_NotReturnsProducts()
    {
        var categories = new List<int> { 2 }; // Categoría 2 no tiene productos
        var products = productsInstance.GetProductsByCategoryIDs(categories);
        Assert.IsNotNull(products);
        Assert.AreEqual(0, products.Count());
    }

    [Test]
    public async Task GetProductsBycategoryID_WithInvalidCategoryID_ReturnsEmpty()
    {
        var categories = new List<int> { 999 }; // Categoría inválida
        var products = productsInstance.GetProductsByCategoryIDs(categories);
        Assert.IsNotNull(products);
        Assert.IsEmpty(products);
    }

    [Test]
    public async Task GetProductsBycategoryID_WithInvalidNegativeCategoryID_ReturnsThrows()
    {
        var categories = new List<int> { -1111 }; // Categoría inválida
        Assert.Throws<ArgumentException>(() => productsInstance.GetProductsByCategoryIDs(categories));
    }


    [Test]
    public async Task GetProductsByCategories_WithEmptyCategories_ThrowsArgumentException()
    {
        var categories = new List<int>();
        Assert.Throws<ArgumentException>(() => productsInstance.GetProductsByCategoryIDs(categories));
    }

    [Test]
    public async Task GetProductsBycategoryID_WithInvalidCategoryID_R()
    {
        var categories = new List<int> { 1, 3, 4 }; // Categoría 3 no tiene asociados,
        // categoria 1 tiene 1 , categoria 4 tiene 3 
        var products = productsInstance.GetProductsByCategoryIDs(categories);
        Assert.IsNotNull(products);
        Assert.AreEqual(4, products.Count());
    }

    [Test]
    public async Task GetProductsBycategoryID_WithInvalidCategoryID_EmptyReturn0()
    {
        var categories = new List<int> { 3, 7 }; // Categoría 3 no tiene asociados, ni la 7
        var products = productsInstance.GetProductsByCategoryIDs(categories);
        Assert.AreEqual(0, products.Count());
    }

    [Test]
    public async Task GetProductsBytextCategory_WithInvalidCategoryID_Return0()
    {
        var categories = new List<int> { 3, 7 }; // Categoría 3,7 no tiene asociados
        var products = productsInstance.SearchByTextAndCategory("Prod",categories);
        Assert.AreEqual(0, products.Count());
    }

}
