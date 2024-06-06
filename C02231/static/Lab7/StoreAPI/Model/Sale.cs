namespace StoreAPI;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }

   // public PaymentMethods PaymentMethod {get;}

    public Sale(IEnumerable<Product> products, string address, decimal Amount)
    {
        Products = products;
        Address = address;
        Amount = Amount;
    }
}