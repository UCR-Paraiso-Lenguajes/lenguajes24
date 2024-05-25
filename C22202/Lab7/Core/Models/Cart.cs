namespace ShopApi.Models;
public sealed class Cart
{
    public required List<string> productsIds { get; set; }
    public float subtotal {get; set;}
    public required string address { get; set; }

    public required PaymentMethods.Type paymentMethod {get; set;}
}