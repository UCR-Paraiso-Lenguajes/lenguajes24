namespace StoreApi.Models
{
public class Product : ICloneable
{
    public string name { get; set; }
    public string imageUrl { get; set; }
    public decimal price { get; set; }
    public string description { get; set; }
    public int quantity {get; set;}
    public Guid Uuid { get; set; }

        // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            Uuid = this.Uuid,
            name = this.name,
            imageUrl = this.imageUrl,
            price = this.price,
            description = this.description,
            quantity = this.quantity

        };
    }
}
}