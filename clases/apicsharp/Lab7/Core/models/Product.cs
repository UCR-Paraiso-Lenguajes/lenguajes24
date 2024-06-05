namespace TodoApi.models;
public class Product : ICloneable
{
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public Guid Uuid { get; set; }
    public int CategoriaId { get; internal set; }

    // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            Uuid = this.Uuid,
            Name = this.Name,
            ImageUrl = this.ImageUrl,
            Price = this.Price,
            Description = this.Description
        };
    }
}