using Core;
using StoreAPI.Business;
using StoreAPI.Database;
using StoreAPI.models;

namespace UT;

public class CategoriesTest
{

    private SaleReportLogic saleReportLogic;
    private StoreDB storeDB;
    private Categories categories;
    private Store store;



    [SetUp]
    public async Task Setup()
    {
        string connectionString = "Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;";
        Storage.Init(connectionString);
        StoreDB.CreateMysql();
        saleReportLogic = new SaleReportLogic();

        // Inicializar las instancias necesarias para las pruebas
        // Inicializar las instancias neces arias para las pruebas
        storeDB = new StoreDB();
        store = await Store.Instance.Value; // Aquí obtienes la instancia existente de Store

        // Asegúrate de que la instancia de Categories se inicialice correctamente
        categories = new Categories(); // Suponiendo que Categories toma un StoreDB en el constructor
    }

    [Test]
    public void GetCategories_ReturnsCategories()
    {
        // Arrange
        var expectedCategories = new List<Category>
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

        // Act
        var result = categories.GetCategories(); //esta

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedCategories.Count, result.Count());
    }


    [Test]
    public void GetCategories_ReturnsCorrectCount()
    {
        // Arrange
        int expectedCount = 10; // Suponiendo que hay 9 categorías en la lista

        // Act
        var _categories = categories.GetCategories(); //esta

        // Assert
        Assert.That(_categories.Count(), Is.EqualTo(expectedCount));
    }

    [Test]
    public void GetCategories_ReturnsNotNull()
    {
        // Act
        var _categories = categories.GetCategories(); //esta

        // Assert
        Assert.IsNotNull(_categories);
        Assert.IsTrue(_categories.Any());
    }

    [Test]
    public void GetCategories_ReturnsOrderedCategories()
    {
        // Arrange
        var expectedCategories = new List<string> { "Adventure", "Dystopian", "Fantasy", "Fiction", "Gift", "Mystery", "NonFiction", "Romance", "Science Fiction", "Young Adult" };

        // Act
        var _categories = categories.GetCategories(); //esta

        // Assert
        CollectionAssert.AreEqual(expectedCategories, _categories.Select(c => c.Name));
    }

}

