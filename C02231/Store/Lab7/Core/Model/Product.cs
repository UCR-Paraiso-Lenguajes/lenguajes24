namespace StoreAPI.models;
public class Product : ICloneable
{
    public required string Name { get; private set; }
    public required string Author { get; private set; }
    public required string ImgUrl { get; private set; }
    public decimal Price { get;private set; }
    public int Id { get; private set; }
    public int Quantity { get; private set; }
     public Category ProductCategory { get; private set; } 


    // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            Id = this.Id,
            Name = this.Name,
            Author = this.Author,
            ImgUrl = this.ImgUrl,
            Price = this.Price,
            Quantity = this.Quantity,
            ProductCategory = this.ProductCategory
        };
    }
}
