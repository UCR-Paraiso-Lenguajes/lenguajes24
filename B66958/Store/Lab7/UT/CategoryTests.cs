using ApiLab7;

namespace UT;

public class CategoryTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        Db.BuildDb(
            "Data Source=163.178.173.130;User ID=basesdedatos;Password=BaSesrp.2024; Encrypt=False;"
        );
    }

    [Test]
    public void CategoryWithIdZero_ThrowsArgumentException()
    {
        int category = 0;
        string name = "category";
        Assert.Throws<ArgumentException>(() => Category.Build(category, name));
    }

    [Test]
    public void CategoryWithNoName_ThrowsArgumentException()
    {
        int category = 1;
        string name = null;
        Assert.Throws<ArgumentException>(() => Category.Build(category, name));
    }

    [Test]
    public void CategoryWithNameFullOfWhiteSpaces_ThrowsArgumentException()
    {
        int category = 1;
        string name = "    ";
        Assert.Throws<ArgumentException>(() => Category.Build(category, name));
    }

    [Test]
    public void CategoryHasValidArguments_DoesNotThrowsArgumentException()
    {
        int category = 1;
        string name = "category";
        Assert.DoesNotThrow(() => Category.Build(category, name));
    }

    [Test]
    public void ProductsByCategory_DoesNotThrowsArgumentExceptionAndHasProducts()
    {
        List<Category> categories = Categories.Instance.GetCategories().ToList();
        List<int> categoriesToSearch = new List<int>();
        categoriesToSearch.Add(categories.ElementAt(0).Id);
        categoriesToSearch.Add(categories.ElementAt(3).Id);
        Assert.DoesNotThrow(() => Store.Instance.ProductsByCategory(categoriesToSearch));
        CollectionAssert.IsNotEmpty(Store.Instance.ProductsByCategory(categoriesToSearch));
    }
}
