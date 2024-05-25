namespace storeApi;
public class Product : ICloneable
{
    public string Name { get; set; }
    public string ImageURL { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public Guid Uuid { get; set; }
    public int Id { get; set; }

        // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {

            Id = this.Id,
            Name =this.Name,
            Description = this.Description,
            Price = this.Price,
            ImageURL = this.ImageURL

        };
    }
}