namespace TodoApi;
public class Product : ICloneable
{
    public int id { get; set; }
     public required string name { get; set; }
    public required string image { get; set; }
    public decimal price { get; set; }
    public required string description { get; set; }

      public object Clone() 
    {
        return new Product
        {
            id=this.id,
            name = this.name,
            image = this.image,
            price = this.price,
            description = this.description
        };
    }
}