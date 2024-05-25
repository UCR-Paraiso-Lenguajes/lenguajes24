namespace ApiLab7;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods PaymentMethod { get; }
    public string PurchaseNumber { get; }

    public Sale(IEnumerable<Product> products, string address, decimal amount, 
        PaymentMethods paymentMethod, string purchaseNumber)
    {
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PurchaseNumber = purchaseNumber;
    }
}