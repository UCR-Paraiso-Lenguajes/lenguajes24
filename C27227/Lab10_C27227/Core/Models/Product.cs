namespace KEStoreApi;
public class Product : ICloneable
{
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int Quantity { get; set;}

            public object Clone()
    {
        return new Product
        {
            Id = this.Id,
            Name = this.Name,
            Price = this.Price,
            ImageUrl = this.ImageUrl,
            Quantity = this.Quantity
        };
    }

      public class ProductQuantity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}