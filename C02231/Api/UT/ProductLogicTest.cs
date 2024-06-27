using System.Security.Cryptography.X509Certificates;
using Core;
using StoreAPI.Business;
using StoreAPI.Database;
using StoreAPI.models;


namespace UT
{

  public class ProductLogicTest
  {

    private ProductLogic productLogic;
    private ProductBD productBD;
    private Products products;
    private List<Category> categoryList;
    [SetUp]
    public async Task SetupAsync()
    {
      string connectionString = "Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;";
      Storage.Init(connectionString);
      StoreDB.CreateMysql();

      productBD = new ProductBD();
      productLogic = new ProductLogic();
      products = Products.Instance;
      await Products.InitializeInstanceAsync();

      categoryList = new List<Category>
        {
            new(1, "Fantasy"),
            new (2, "Romance"),
            new (3, "Science Fiction"),
            new (4, "Young Adult"),
            new (5, "Mystery"),
            new (6, "NonFiction"),
            new (7, "Fiction"),
            new (8, "Adventure"),
            new (9, "Dystopian"),
            new (10, "Gift")
        };

    }

    [Test]
    public async Task AddProduct_ValidProduct_ShouldAddProduct()
    {
      // Arrange
      var product = new Product
      {
        Name = "Test Product",
        Description = "Test Description",
        ImgUrl = "http://example.com/img.png",
        Price = 100,
        ProductCategory = categoryList.Single(category => category.IdCategory == 8)
      };

      // Act
      var result = await productLogic.AddProduct(product);

      // Assert
      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("Test Product", result.Name);

    }

    [Test]
    public async Task AddProduct_NullProduct_ShouldThrowException()
    {
      // Arrange
      Product product = null;

      // Act & Assert
      Exception ex = null;
      try
      {
        await productLogic.AddProduct(product);
      }
      catch (Exception e)
      {
        ex = e;
      }

      Assert.IsNotNull(ex);
      Assert.AreEqual("The product cannot be null.", ex.Message);
    }

  }
}
