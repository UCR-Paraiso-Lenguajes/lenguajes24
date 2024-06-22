using Core.Models;

namespace Store_API.Models;

public class Product : ICloneable
{
    public string Name { get; set; }
    public string ImageURL { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Id { get; set; }
    public Category Categoria {get; set;}

    // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            Id = this.Id,
            Name = this.Name,
            ImageURL = this.ImageURL,
            Description= this.Description,
            Price = this.Price,
            Categoria  = this.Categoria 
        };
    }
}