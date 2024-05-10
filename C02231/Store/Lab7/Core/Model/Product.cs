namespace StoreAPI.models;
public class Product : ICloneable
{
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string ImgUrl { get; set; }
    public decimal Price { get; set; }
    public int Id {get; set;}
     public int Quantity { get; set; }
    
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
            Quantity =this.Quantity
            
        };
    }
}