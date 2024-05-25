using System.Security.Cryptography.X509Certificates;
using Core;
using StoreAPI.models;

namespace UT;
public class ProductTest
{

    private Products products;



    [SetUp]
    public async Task SetupAsync()
    {
        string connectionString = "Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;";
        Storage.Init(connectionString);

        var loadedProducts = await Store.LoadProductsAsync();
        var productByName = new Dictionary<string, List<Product>>();
        var productByCategoryId = new Dictionary<int, List<Product>>();
        foreach (var product in loadedProducts)
        {
            if (!productByCategoryId.TryGetValue(product.ProductCategory.IdCategory, out var productCategoryId))
            {
                productCategoryId = new List<Product>();
                productByCategoryId[product.ProductCategory.IdCategory] = productCategoryId;
            }
            productCategoryId.Add(product);
            if (!productByName.ContainsKey(product.Name))
            {
                productByName[product.Name] = new List<Product>();
            }
            productByName[product.Name].Add(product);
        }


        products = new Products(loadedProducts, productByCategoryId, productByName);
    }


    [Test]
    public async Task SearchProducts_WithValidKeywords_ReturnsMatchingProducts()
    {
        // Arrange
        var categoryIds = new List<int> { 1, 2, 3 };
        var keywords = "cinder";
        var expectedCount = 1;

        // Act
        var matchingProducts = await products.SearchProductsAsync(categoryIds, keywords);
        var actualCount = matchingProducts.Count();

        // Assert
        Assert.IsNotNull(matchingProducts);
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task SearchProducts_WithValidKeywords_ReturnsMatchingProductsAuthor()
    {
        // Arrange
        var categoryIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var keywords = "a";
        var expectedCount = 34;

        // Act
        var matchingProducts = await products.SearchProductsAsync(categoryIds, keywords);
        var actualCount = matchingProducts.Count();
        // Assert
        Assert.IsNotNull(matchingProducts);
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task SearchProducts_WithValidKeywords_ReturnsMatchingProductsH()
    {
        // Arrange
        var categoryIds = new List<int> { 1, 2 };
        var keywords = "harry";
        var expectedCount = 7;

        // Act
        var matchingProducts = await products.SearchProductsAsync(categoryIds, keywords);
        var actualCount = matchingProducts.Count();

        // Assert
        Assert.IsNotNull(matchingProducts);
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }


    [Test]
    public void SearchProducts_WithInvalidCategory_ThrowsArgumentException()
    {
        // Arrange
        var invalidCategoryIds = new List<int> { -1, 0 };
        var keywords = "book";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => products.SearchProductsAsync(invalidCategoryIds, keywords).GetAwaiter().GetResult());
    }

}

