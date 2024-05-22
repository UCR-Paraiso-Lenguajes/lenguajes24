using TodoApi;
using TodoApi.Models;

namespace TodoApi;
public sealed class Cart
{
    public List<string> ProductIds { get; private set; }
    public string Address { get; private set; }
    public PaymentMethod.Type PaymentMethod { get; private set; }
    public decimal Total { get; private set; }

    public Cart(List<string> productIds, string address, PaymentMethod.Type paymentMethod, decimal total)
    {
        ProductIds = productIds;
        Address = address;
        PaymentMethod = paymentMethod;
        Total = total;
    }

}