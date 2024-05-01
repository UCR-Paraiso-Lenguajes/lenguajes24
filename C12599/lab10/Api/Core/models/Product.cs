using System;

namespace storeapi.Models
{
    public class Product : ICloneable
    {
        public int id { get; set; }
        private string? _name;
        private string? _imageUrl;
        private decimal _price;
        private string? _description;

        public string? Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name must not be null or empty.");
                }
                _name = value;
            }
        }

        public string? ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ImageUrl must not be null or empty.");
                }
                _imageUrl = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Price must be a positive decimal value.");
                }
                _price = value;
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Description must not be null or empty.");
                }
                _description = value;
            }
        }

        public object Clone()
        {
            return new Product
            {
                id = this.id,
                Name = this.Name,
                ImageUrl = this.ImageUrl,
                Price = this.Price,
                Description = this.Description
            };
        }
    }
}
