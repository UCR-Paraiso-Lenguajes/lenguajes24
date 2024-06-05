namespace ApiLab7;

public sealed class Store
{
    public List<Product> ProductsInStore { get; private set; }
    public int TaxPercentage { get; private set; }
    public List<PaymentMethods> paymentMethods { get; private set; }
    public IEnumerable<Category> CategoriesInStore { get; private set; }
    public static int CurrentTaxPercentage = 13;
    private static Db db;

    private Store(
        List<Product> products,
        int TaxPercentage,
        List<PaymentMethods> paymentMethods,
        IEnumerable<Category> categories
    )
    {
        this.ProductsInStore = products;
        this.TaxPercentage = TaxPercentage;
        this.paymentMethods = paymentMethods;
        this.CategoriesInStore = categories;
    }

    public static readonly Store Instance;

    public IEnumerable<Product> ProductsByCategoryAndQuery(List<int> categories, string query)
    {
        if (categories == null || categories.Count() == 0)
            throw new ArgumentException("The categories list should not be null");
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("The query should not be null");
        return Products.Instance.GetProductsByCategoryAndQuery(categories, query);
    }

    public IEnumerable<Product> ProductsByCategory(List<int> categories)
    {
        if (categories == null || categories.Count() == 0)
            throw new ArgumentException("The categories list should not be null");
        return Products.Instance.GetProductsByCategory(categories);
    }

    public IEnumerable<Product> ProductsByQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("The query should not be null");
        return Products.Instance.GetProductsByQuery(query);
    }

    static Store()
    {
        db = Db.Instance;
        List<Product> products = Db.GetProducts();
        List<PaymentMethods> paymentMethods = db.GetPaymentMethods();
        IEnumerable<Category> categories = Categories.Instance.GetCategories();

        Store.Instance = new Store(
            products,
            Store.CurrentTaxPercentage,
            paymentMethods,
            categories
        );
    }
}
