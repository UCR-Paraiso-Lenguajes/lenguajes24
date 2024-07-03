namespace TodoApi.Models
{
    public sealed class Store
    {
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }

        private Store(List<Product> products, int TaxPercentage)
        {
            this.Products = products;
            this.TaxPercentage = TaxPercentage;
        }

        public readonly static Store Instance;
        // Static constructor
        static Store()
        {

            var products = new List<Product>
            {
                new Product
                {
                    Id= 1,
                    Name= "Producto 1",
                    Description= "Descripción 1",
                    ImageURL= "https://images-na.ssl-images-amazon.com/images/I/71JSM9i1bQL.AC_UL160_SR160,160.jpg",
                    Price= 10
                },
                new Product
                {
                    Id= 2,
                    Name= "Producto 2",
                    Description= "Descripción 2",
                    ImageURL= "https://images-na.ssl-images-amazon.com/images/I/418UoVylqyL._AC_UL160_SR160,160_.jpg",
                    Price= 20
                },
                new Product
                {
                    Id= 3,
                    Name= "Producto 3",
                    Description= "Descripción 3",
                    ImageURL= "https://images-na.ssl-images-amazon.com/images/I/81WsSyAYxHL._AC_UL160_SR160,160_.jpg",
                    Price= 30
                },
                new Product
                {
                    Id= 4,
                    Name= "Producto 4",
                    Description= "Descripción 4",
                    ImageURL= "https://images-na.ssl-images-amazon.com/images/I/51-lOBlIrFL._AC_UL160_SR160,160_.jpg",
                    Price= 40
                },
                new Product
                {
                    Id= 5,
                    Name= "Producto 2",
                    Description= "Descripción 5",
                    ImageURL= "https://images-na.ssl-images-amazon.com/images/I/51wD-xrtyWL._AC_UL160_SR160,160_.jpg",
                    Price= 50
                },

                new Product
                {
                    Id= 6,
                    Name= "Producto 6",
                    Description= "Descripción 6",
                    ImageURL= "https://images-na.ssl-images-amazon.com/images/I/71EZAE6fljL._AC_UL160_SR160,160_.jpg",
                    Price= 60
                },
                new Product
                {
                    Id= 7,
                    Name= "Producto 7",
                    Description= "Descripción 7",
                    ImageURL= "https://m.media-amazon.com/images/I/817EyM89DtL._AC_SY100_.jpg",
                    Price= 70
                },
                new Product
                {
                    Id= 8,
                    Name= "Producto 8",
                    Description= "Descripción 8",
                    ImageURL= "https://m.media-amazon.com/images/I/61J0e7d0GEL._AC_SY100_.jpg",
                    Price= 80
                },
                new Product
                {
                    Id= 9,
                    Name= "Producto 9",
                    Description= "Descripción 9",
                    ImageURL= "https://m.media-amazon.com/images/I/81mzvAGkHkL._AC_SY100_.jpg",
                    Price= 90
                },
                new Product
                {
                    Id= 10,
                    Name= "Producto 10",
                    Description= "Descripción 10",
                    ImageURL= "https://m.media-amazon.com/images/I/51YlAYwPx6L._AC_SY100_.jpg",
                    Price= 100
                },
                new Product
                {
                    Id= 11,
                    Name= "Producto 11",
                    Description= "Descripción 11",
                    ImageURL= "https://m.media-amazon.com/images/I/71cj5cNm7ZL._AC_UY218_.jpg",
                    Price= 110
                },
                new Product
                {
                    Id= 12,
                    Name= "Producto 12",
                    Description= "Descripción 12",
                    ImageURL= "https://m.media-amazon.com/images/I/7148mbvrbWL._AC_UL320_.jpg",
                    Price= 120
                },
                new Product
                {
                    Id = 13,
                    Name = "Producto 12",
                    Description = "Descripción 13",
                    ImageURL = "https://m.media-amazon.com/images/I/71Pf0aGicBL._AC_UY218_.jpg",
                    Price = 130
                },
                new Product
                {
                    Id= 14,
                    Name= "Producto 14",
                    Description= "Descripción 14",
                    ImageURL= "https://m.media-amazon.com/images/I/71P84KYUfrL._AC_UL320_.jpg",
                    Price= 140
                },
                new Product
                {
                    Id= 15,
                    Name= "Producto 15",
                    Description= "Descripción 15",
                    ImageURL= "https://m.media-amazon.com/images/I/51gJxciP-qL._AC_UY218_T2F_.jpg",
                    Price= 150
                },
                new Product
                {
                    Id= 16,
                    Name= "Producto 16",
                    Description= "Descripción 16",
                    ImageURL= "https://m.media-amazon.com/images/I/61OI1MNjZZL._AC_UY218_T2F_.jpg",
                    Price= 160
                }
            };

            Store.Instance = new Store(products, 13);
        }

        public Sale Purchase(Cart cart)
        {
            if (cart.ProductIds.Count == 0) throw new ArgumentException("Cart must contain at least one product.");
            if (string.IsNullOrWhiteSpace(cart.Address)) throw new ArgumentException("Address must be provIded.");

            // Find matching products based on the product Ids in the cart
            IEnumerable<Product> matchingProducts = Products.Where(p => cart.ProductIds.Contains(p.Id.ToString())).ToList();

            // Create shadow copies of the matching products
            IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            // Calculate purchase amount by multiplying each product's Price with the store's tax percentage
            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                product.Price *= (1 + (decimal)TaxPercentage / 100);
                purchaseAmount += product.Price;
            }

            PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);

            // Create a sale object
            var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount);

            return sale;

        }
    }
}