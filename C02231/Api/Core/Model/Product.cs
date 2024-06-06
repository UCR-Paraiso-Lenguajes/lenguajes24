namespace StoreAPI.models;
public class Product : ICloneable
{
    public string Name { get; private set; }
    public string Author { get; private set; }
    public string ImgUrl { get; private set; }
    public decimal Price { get; set; }
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Category ProductCategory { get; set; }

    public Product(string name, string author, string imgUrl, decimal price, Category category, int id)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        if (string.IsNullOrEmpty(author)) throw new ArgumentException("Description cannot be null or empty.", nameof(author));
        if (string.IsNullOrEmpty(imgUrl)) throw new ArgumentException("ImageURL cannot be null or empty.", nameof(imgUrl));
        if (price < 0) throw new ArgumentException("Price must be greater than zero.", nameof(price));
        if (id < 0) throw new ArgumentException("Id must be greater than zero.", nameof(id));

        Name = name;
        Author = author;
        ImgUrl = imgUrl;
        Price = price;
        Id = id;
        ProductCategory = category;
    }
    // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product(this.Name, this.Author, this.ImgUrl, this.Price, this.ProductCategory, this.Id);
    }
}
