namespace storeApi;
public class Product : ICloneable
{
    public required string Name { get; set; }
    public required string ImageURL { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public Guid Uuid { get; set; }
    public int Id { get; set; }
    public ProductCategory Category { get; set; } 

    public object Clone()
    {
        return new Product
        {

            Id = this.Id,
            Name =this.Name,
            Description = this.Description,
            Price = this.Price,
            ImageURL = this.ImageURL,
            Category = this.Category

        };
    }
}