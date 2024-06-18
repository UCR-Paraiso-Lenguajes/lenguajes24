namespace TodoApi.Models
{
    public class Product : ICloneable
    {
        public string Name { get; private set; }
        public string ImageURL { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int Id { get; private set; }
        public Categories.CategorySt Category { get; private set; }

        public Product(string name, string imageURL, decimal price, string description, int id, Categories.CategorySt category)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty.");
            if (string.IsNullOrEmpty(imageURL)) throw new ArgumentException("ImageURL cannot be null or empty.");
            if (price <= 0) throw new ArgumentException("Price must be greater than zero.");
            if (string.IsNullOrEmpty(description)) throw new ArgumentException("Description cannot be null or empty.");
            if (id <= 0) throw new ArgumentException("Id must be greater than zero.");
            if (category.Equals(default(Categories.CategorySt))) throw new ArgumentException("Category cannot be null.");

            Name = name;
            ImageURL = imageURL;
            Price = price;
            Description = description;
            Id = id;
            Category = category;
        }

        // Implementation of the ICloneable interface
        public object Clone()
        {
            return new Product(this.Name, this.ImageURL, this.Price, this.Description, this.Id, this.Category);
        }
    }
}