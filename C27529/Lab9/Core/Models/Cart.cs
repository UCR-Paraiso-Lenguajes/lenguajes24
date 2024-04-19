using storeApi.Models;

namespace storeApi;
public sealed class Cart
{
    public required List<string> ProductIds { get; set; }
    public required string Address { get; set; }
    public PaymentMethod.Type PaymentMethod{get; set;}
    public decimal Total {get; set;}
}