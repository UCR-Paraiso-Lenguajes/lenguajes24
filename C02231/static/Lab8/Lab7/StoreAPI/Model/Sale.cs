using System.Text;

namespace StoreAPI.models;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public decimal Amount { get; }
    public PaymentMethods.Type PaymentMethod { get; }
    public string NumberOrder { get; }
 

    public Sale(IEnumerable<Product> products, string address, decimal amount, PaymentMethods.Type paymentMethod, string numberOrder)
    {
        Products = products;
        Address = address;
        Amount = amount;
        PaymentMethod = paymentMethod;
        NumberOrder = numberOrder;
    }
}


