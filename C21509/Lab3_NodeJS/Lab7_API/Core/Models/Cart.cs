namespace Store_API.Models;
using System.Text.Json.Serialization;

public sealed class Cart
{
    public List<ProductQuantity> Products { get; set; }
    public string Address { get; set; }
    
    [JsonPropertyName("paymentMethod")]
    public PaymentMethods.Type PaymentMethod { get; set; }
    public decimal Total { get; set; }
    public decimal Subtotal { get; set; } 

    public Cart(List<ProductQuantity> products, string address, PaymentMethods.Type paymentMethod, decimal total, decimal subtotal)
    {
        Products = products;
        Address = address;
        PaymentMethod = paymentMethod;
        Total = total;
        Subtotal = subtotal;
    }
}

public class ProductQuantity
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; } 

    public ProductQuantity(int id, int quantity, decimal price)
    {
        Id = id;
        Quantity = quantity;
        Price = price;
    }
}