namespace storeApi.Models;
public sealed class Cart
{
    public IEnumerable<ProductQuantity> ProductIds { get; set; }
    public string Address { get; set; }
    public PaymentMethods.Type PaymentMethod { get; set; }
}
