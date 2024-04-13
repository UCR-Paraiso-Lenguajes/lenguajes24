using System.Text;

namespace geekstore_api;
public sealed class Sale
{
    public IEnumerable<Product> Products { get; }
    public string Address { get; }
    public readonly decimal amount;
    public PaymentMethods PaymentMethod { get; }

    public decimal Amount(){  return amount; }

    public Sale(IEnumerable<Product> products, string address, decimal Amount, PaymentMethods paymentMethod )
    {
        Products = products;
        Address = address;
        amount = Amount;
        PaymentMethod = paymentMethod;
    }

}