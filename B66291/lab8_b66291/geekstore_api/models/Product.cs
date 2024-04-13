namespace geekstore_api;
public class Product : ICloneable
{
    public int id {get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public decimal price { get; set; }
    public string imageUrl { get; set; }
   

    // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            id = this.id,
            name = this.name,
            description = this.description,
            price = this.price,
            imageUrl = this.imageUrl,
        };
    }
}