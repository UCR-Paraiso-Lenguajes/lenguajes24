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

    public Product() { }
   public Product(string name, string imageURL, decimal price, string description, int id, Category categoria)
        {
            Name = name;
            ImageURL = imageURL;
            Price = price;
            Description = description;
            Id = id;
            Categoria = categoria;
        }

        public object Clone()
        {
            return new Product(Name, ImageURL, Price, Description, Id, Categoria);
        }
}