namespace StoreAPI.models;
using System.ComponentModel.DataAnnotations;
public class Product : ICloneable
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string ImgUrl { get; set; }
    [Required]
    public decimal Price { get; set; }
    public int Id { get; set; }
    public int Quantity { get; set; }
    [Required]
    public Category ProductCategory { get; set; }

    public Product(string name, string description, string imgUrl, decimal price, Category category, int id)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{nameof(name)} cannot be null or empty.");
        if (string.IsNullOrEmpty(description)) throw new ArgumentException($"{nameof(description)} cannot be null or empty.");
        if (string.IsNullOrEmpty(imgUrl)) throw new ArgumentException($"{nameof(imgUrl)} cannot be null or empty.");
        if (price < 0) throw new ArgumentException($"{nameof(price)} must be greater than zero.");
        if (id < 0) throw new ArgumentException($"{nameof(id)} must be greater than zero.");

        Name = name;
        Description = description;
        ImgUrl = imgUrl;
        Price = price;
        Id = id;
        ProductCategory = category;
    }

    public Product()
        {
        }
    // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product(this.Name, this.Description, this.ImgUrl, this.Price, this.ProductCategory, this.Id);
    }
}

public class ProductQuantity
{
    public string ProductId { get; private set; }
    public int Quantity { get; private set; }
    public ProductQuantity(string productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId)) { throw new ArgumentException($"The {nameof(productId)} cant be null or empty."); }
        if (quantity < 0) { throw new ArgumentOutOfRangeException($" {nameof(quantity)} cant be lower than 0."); }
        ProductId = productId;
        Quantity = quantity;
    }
}
