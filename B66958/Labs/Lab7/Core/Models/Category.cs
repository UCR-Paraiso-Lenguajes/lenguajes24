namespace ApiLab7;

public readonly struct Category
{
    public int Id { get; }
    public string Name { get; }

    private Category(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Category Build(int id, string name)
    {
        if (id == null || id < 1)
            throw new ArgumentException("A category must have an ID, and it should be above 0");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A category must have name");
        return new Category(id, name);
    }
}

public class Categories
{
    private static Categories instance;
    private List<Category> categories;

    public static Categories Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Categories();
                instance.BuildCategories();
            }
            return instance;
        }
    }

    private Categories()
    {
        categories = new List<Category>();
    }

    private void BuildCategories()
    {
        AddCategory(1, "Deportes");
        AddCategory(2, "Entretenimiento");
        AddCategory(3, "Hogar");
        AddCategory(4, "Música");
        AddCategory(5, "Cuidado personal");

        categories.Sort((category1, category2) => string.Compare(category1.Name, category2.Name));
    }

    private void AddCategory(int id, string name)
    {
        Category newCategory = Category.Build(id, name);
        categories.Add(newCategory);
    }

    public IEnumerable<Category> GetCategories()
    {
        return categories;
    }

    public Category GetCategoryById(int id)
    {
        if (id < 1)
            throw new ArgumentException("A category must have an ID, and it should be above 0");
        return categories.FirstOrDefault(category => category.Id == id);
    }
}
