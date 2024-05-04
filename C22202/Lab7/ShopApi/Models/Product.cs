namespace ShopApi.Models;
public class Product : ICloneable
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int id {get; set;}

        // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            id = this.id,
            Name = this.Name,
            ImageUrl = this.ImageUrl,
            Price = this.Price,
            Description = this.Description
        };
    }
}