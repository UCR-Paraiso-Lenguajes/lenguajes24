namespace TodoApi.Models;
public class Product : ICloneable
{
    public required string Name { get; set; }
    public required string ImageURL { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public int Id { get; set; }

        // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            Id = this.Id,
            Name = this.Name,
            ImageURL = this.ImageURL,
            Price = this.Price,
            Description = this.Description
        };
    }
}