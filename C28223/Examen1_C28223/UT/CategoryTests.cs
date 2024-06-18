namespace UT;
using Core;
using storeApi.Models.Data;
public class CategoryTests
{
    Categories category;
    [SetUp]
    public void Setup()
    {
        category = new Categories();
    }

    [Test]
    public void CreateCategory_Struct_CreatesCategoryWithValidData()
    {
        Category category = new Category();
        Category createdCategory = category.crearCategoria(1, "Electronica");
        Assert.AreEqual(1, createdCategory.CategoryID);
        Assert.AreEqual("Electronica", createdCategory.NameCategory);
    }

    [Test]
    public void GetCategories_ReturnsOrderedCategories()
    {
        var categories = category.GetCategories();
        var expectedCategories = new List<string> { "Actividades al aire libre", "Alimentación", "Belleza", "Deportes", "Electrónica", "Entretenimiento", "Hogar y jardín", "Moda", "Tecnología" };
        CollectionAssert.AreEqual(expectedCategories, categories.Select(cat => cat.NameCategory).ToList());
    }
    [Test]
    public void CreateCategory_Struct_ThrowsExceptionWithInvalidData()
    {
        Category category = new Category();
        Assert.Throws<ArgumentException>(() => category.crearCategoria(-1, "Invalid Category"));
        Assert.Throws<ArgumentException>(() => category.crearCategoria(1, ""));
    }
    [Test]
    public void CreateCategory_Struct_ThrowsExceptionForInvalidName()
    {
        Category category = new Category();
        Assert.Throws<ArgumentException>(() => category.crearCategoria(10, null));
        Assert.Throws<ArgumentException>(() => category.crearCategoria(10, ""));
    }

    [Test]
    public void AddCategories_CheckCount_ReturnsCorrectCount()
    {
        int expectedCount = 9;
        var categories = category.GetCategories();
        Assert.AreEqual(expectedCount, categories.Count());
    }

    [Test]
    public void GetCategoryById_WithValidID_ReturnsCorrectCategory()
    {
        int categoryId = 3;
        string expectedName = "Hogar y jardín";
        var categoryById = category.GetCategoryById(categoryId);
        Assert.AreEqual(expectedName, categoryById.NameCategory);
        Assert.AreEqual(categoryId, categoryById.CategoryID);
    }
    [Test]
    public void GetCategoryById_WithInvalidID_ThrowsException()
    {
        int invalidId = 0;
        int invalidIdNegative = -1;
        Assert.Throws<ArgumentException>(() => category.GetCategoryById(invalidId));
        Assert.Throws<ArgumentException>(() => category.GetCategoryById(invalidIdNegative));
    }
    [Test]
    public void GetCategoryById_IDNotInList_ThrowsException()
    {
        int idNotInList = 999999999;
        Assert.Throws<InvalidOperationException>(() => category.GetCategoryById(idNotInList));
    }
    [Test]
    public void GetCategories_OrderByName_ReturnsSortedCategories()
    {
        //expectedCategories  ya esta ordenada
        var expectedCategories = new List<string> { "Actividades al aire libre", "Alimentación", "Belleza", "Deportes", "Electrónica", "Entretenimiento", "Hogar y jardín", "Moda", "Tecnología" };
        var categories = category.GetCategories().Select(cat => cat.NameCategory).ToList();
        CollectionAssert.AreEqual(expectedCategories, categories);
    }
}
