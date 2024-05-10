namespace Store_API.Models;
using System.Text.Json.Serialization;

public sealed class Cart
{
    public List<int> ProductIds { get; set; }
    public string Address { get; set; }
    
    [JsonPropertyName("paymentMethod")]
    public PaymentMethods.Type PaymentMethod { get; set; }
    public decimal Total { get; set; } 
    public decimal Subtotal { get; set; } 

     public Cart(List<int> productIds, string address, PaymentMethods.Type paymentMethod, decimal total, decimal subtotal)
        {
            ProductIds = productIds;
            Address = address;
            PaymentMethod = paymentMethod;
            Total = total;
            Subtotal = subtotal;
        }
}