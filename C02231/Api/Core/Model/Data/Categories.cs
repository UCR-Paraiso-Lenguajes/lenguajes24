
namespace StoreAPI.models;


public class Categories
{
    private static Categories categories = new Categories();
    private readonly List<Category> categoryList;

    public static Categories Instance
    {
        get
        {
            if (categories == null) categories = new Categories();

            return categories;
        }
    }

    public Categories()
    {
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

        categoryList.Sort((category1, category2) => string.Compare(category1.Name, category2.Name));
    }

    public IEnumerable<Category> GetCategories()
    {
        if (categoryList.Count == 0)
        {
            throw new Exception("No categories available.");
        }

        return categoryList;
    }
}



public struct Category
{
    public int IdCategory { get; set;}
    public string Name { get; set;}
    public Category(int idCategory, string name)
    {
        if (idCategory <= -1) throw new ArgumentException($"A Category {nameof(idCategory)} is required");
        if (name == null) throw new ArgumentException($"The category {nameof(name)} is required");

        IdCategory = idCategory;
        Name = name;
    }

}
