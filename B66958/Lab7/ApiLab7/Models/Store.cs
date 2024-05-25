namespace ApiLab7;
public sealed class Store
{
    public List<Product> Products { get; private set; }
    public int TaxPercentage { get; private set; }
    public List<PaymentMethods> paymentMethods{ get; private set; }
    public bool InitializedStore {get; private set;}
    private static Db db;

    private Store( List<Product> products, int TaxPercentage, List<PaymentMethods> paymentMethods )
    {
        this.Products = products;
        this.TaxPercentage = TaxPercentage;
        this.paymentMethods = paymentMethods;
    }

    public readonly static Store Instance;
    
    static Store()
    {
        db = new Db();
        List<Product> products = db.GetProducts();
        var paymentMethods = new List<PaymentMethods>();
        paymentMethods.Add(Sinpe.Instance);
        paymentMethods.Add(Cash.Instance);

        Store.Instance = new Store(products, 13, paymentMethods);
    }
}