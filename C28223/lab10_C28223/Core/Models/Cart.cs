namespace storeApi.Models;
public sealed class Cart
{
    public List<ProductQuantity> ProductIds { get; set; }
    public string Address { get; set; }
    public PaymentMethods.Type PaymentMethod { get; set; }
}
