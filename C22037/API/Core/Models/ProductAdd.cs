namespace TodoApi.Models
{
    public class ProductAdd
    {
        public string name { get; private set; }
        public string imageUrl { get; private set; }
        public decimal price { get; private set; }
        public string description { get; private set; }
        public int category { get; private set; }
        public int quantity { get; private set; }

        public ProductAdd(string Name, string imageURL, decimal Price, string Description, int Category, int Quantity)
        {
            if (string.IsNullOrEmpty(Name)) throw new ArgumentException("Name cannot be null or empty.");
            if (string.IsNullOrEmpty(imageURL)) throw new ArgumentException("ImageURL cannot be null or empty.");
            if (Price <= 0) throw new ArgumentException("Price must be greater than zero.");
            if (string.IsNullOrEmpty(Description)) throw new ArgumentException("Description cannot be null or empty.");
            if (Category <= 0) throw new ArgumentException("Category must be greater than zero.");
            if (Quantity < 0) throw new ArgumentException("Quantity cannot be less than zero.");

            name = Name;
            imageUrl = imageURL;
            price = Price;
            description = Description;
            category = Category;
            quantity = Quantity;
        }
    }
}