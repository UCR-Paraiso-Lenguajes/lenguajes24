namespace StoreApi.Models
{
  public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods PaymentMethod { get; }

    public Sale(IEnumerable<Product> products, string address, decimal amount, PaymentMethods paymentMethod)
    {
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
    }
}
}