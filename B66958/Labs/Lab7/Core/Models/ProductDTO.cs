namespace ApiLab7;

public sealed class ProductDTO
{
    public string Name { get; set;}
    public string ImageUrl { get; set;}
    public decimal Price { get; set;}
    public string Description { get; set;}
    public int Category { get; set;}

    public Product ToProduct()
    { 
        return new Product{
            Uuid = Guid.NewGuid(),
            Name = Name,
            ImageUrl = ImageUrl,
            Price = Price,
            Description = Description,
            Category = Categories.Instance.GetCategoryById(this.Category)
        };
    }
}