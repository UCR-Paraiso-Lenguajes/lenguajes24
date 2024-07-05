using System;
using Core;

namespace KEStoreApi
{
    public class Product : ICloneable, IComparable<Product>
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }    
    public int Quantity { get; set; }
    public int CategoriaId { get; set; }
     public bool IsDeleted { get; set; }
        public object Clone()
        {
            return new Product
            {
                Id = this.Id,
                Name = this.Name,
                Price = this.Price,
                ImageUrl = this.ImageUrl,
                Description = this.Description,
                Quantity = this.Quantity
            };
        }

        public int CompareTo(Product other)
        {
            if (other == null) return 1;
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return Name;
        }

        public class ProductQuantity
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
        }
    }
}
