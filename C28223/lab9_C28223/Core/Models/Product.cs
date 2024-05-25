namespace storeApi;
public class Product : ICloneable
{
    public string name { get; set; }
    public string  imageURL { get; set; }
    public decimal price { get; set; }
    public string description { get; set; }
    public int id { get; set; }

        // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            id = this.id,
            name = this.name,
            imageURL = this.imageURL,
            price = this.price,
            description = this.description
        };
    }
}