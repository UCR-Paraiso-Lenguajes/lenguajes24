
using MySqlConnector;
using System;
using storeApi.Models;
namespace storeApi
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
                    Id = 1,
                    Name = "Producto 1",
                    Description = "Audífonos con alta fidelidad",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Fuji/2021/June/Fuji_Quad_Headset_1x._SY116_CB667159060_.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Producto 2",
                    Description = "Control PS4",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Controller2.png"
                },
                new Product
                {
                    Id = 3,
                    Name = "Producto 3",
                    Description = "PS4 1TB",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Playstation3.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Producto 4",
                    Description = "Crash Bandicoot 4 Switch",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Game.png"
                },
                new Product
                {
                    Id = 5,
                    Name = "Producto 5",
                    Description = "Mouse Logitech",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_Quad_Mouse.jpg"
                },
                new Product
                {
                    Id = 6,
                    Name = "Producto 6",
                    Description = "Silla Oficina",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_Quad_Chair.jpg"
                },
                new Product
                {
                    Id = 7,
                    Name = "Producto 7",
                    Description = "Laptop Acer",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Laptop.png"
                },
                new Product
                {
                    Id = 8,
                    Name = "Producto 8",
                    Description = "Oculus Quest 3",
                    Price = 20000,
                    ImageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Karu/2021/June/Karu_LP_Oculus2.jpg"
                }
            };

            Store.Instance = new Store(products, 13);
           


        }
        public Sale Purchase(Cart cart)
        {
            if (cart.ProductIds.Count == 0)
                throw new ArgumentException("Cart must contain at least one product.");
            if (string.IsNullOrWhiteSpace(cart.Address))
                throw new ArgumentException("Address must be provided.");

            IEnumerable<Product> matchingProducts = Products.Where(p => cart.ProductIds.Contains(p.Id.ToString())).ToList();

            IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                product.Price *= (1 + (decimal)TaxPercentage / 100);
                purchaseAmount += product.Price;
            }

            // Create a sale object
            var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, cart.PaymentMethod);

            return sale;
        }
    }
}
