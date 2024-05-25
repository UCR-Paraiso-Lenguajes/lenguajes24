namespace ShopApi.Models;

public sealed class Store
{
    public IEnumerable<Product> Products { get; private set; }
    public decimal TaxPercentage { get; private set; }
    public IEnumerable<Category> Categories { get; private set; }
    
    private Store( IEnumerable<Product> products, decimal TaxPercentage, IEnumerable<Category> categories)
    {
        this.Products = products;
        this.TaxPercentage = TaxPercentage;
        this.Categories = categories;
    }

    public readonly static Store Instance;
    static Store()
    {
        var products = ProductsLogic.Instance.products;

        var categories = CategoriesLogic.Instance.GetCategories();

        Store.Instance = new Store(products, 13, categories);
    }
}