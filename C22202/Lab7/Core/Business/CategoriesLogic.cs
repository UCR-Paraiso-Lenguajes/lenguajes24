using ShopApi.Models;
public class CategoriesLogic
{
    private readonly List<Category> categories;
    public static CategoriesLogic Instance;

    private CategoriesLogic(List<Category> categories)
    {
        this.categories = categories;
    }

    static CategoriesLogic()
    {
        var categoryList = new List<Category>
        {
            new Category(1, "Mouse"),
            new Category(2, "Monitores"),
            new Category(3, "SSD"),
            new Category(4, "Discos Duros"),
            new Category(5, "CPU"),
            new Category(6, "Tarjetas Gráficas"),
            new Category(7, "Teclados"),
            new Category(8, "Memorias RAM"),
            new Category(9, "Headsets"),
            new Category(10, "Fuentes de poder"),
            new Category(11, "Cases"),
            new Category(12, "Tarjetas Madre"),
            new Category(13, "Micrófonos")
        };

        categoryList.Sort((category1, category2) => string.Compare(category1.name, category2.name));

        CategoriesLogic.Instance = new CategoriesLogic(categoryList);
    }

    public IEnumerable<Category> GetCategories()
    {
        if (this.categories.Count == 0)
        {
            throw new Exception("No categories available.");
        }

        return this.categories;
    }
}
