
namespace StoreAPI.models;
public sealed class  Cart
{
    //public List<string> ProductIds { get; set; }
    public IEnumerable<ProductQuantity> ProductIds { get; set; }
    public string Address { get; set; }
    public PayMethod PaymentMethod { get; set; }
    public decimal Total { get; set; }
}