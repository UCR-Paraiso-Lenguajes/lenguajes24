namespace TodoApi.Models;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }

    public Sale(IEnumerable<Product> products, string address, decimal amount)
    {
        Products = products;
        Address = address;
        Amount = amount;
    }
}