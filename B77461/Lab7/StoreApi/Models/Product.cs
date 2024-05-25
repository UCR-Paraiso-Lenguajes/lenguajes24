namespace StoreApi.Models
{
public class Product : ICloneable
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Quantity {get; set;}
    public Guid Uuid { get; set; }

        // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            Uuid = this.Uuid,
            Name = this.Name,
            ImageUrl = this.ImageUrl,
            Price = this.Price,
            Description = this.Description,
            Quantity = this.Quantity

        };
    }
}
}