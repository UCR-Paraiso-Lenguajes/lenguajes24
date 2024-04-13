using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;

namespace storeapi
{
    public sealed class Store
    {
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }
        public PaymentMethods MethodPayment { get; private set; }

        private Store(List<Product> products, int taxPercentage)
        {
            Products = products;
            TaxPercentage = taxPercentage;

            var initializer = new DatabaseLab();
            initializer.CreateDatabase();
        }

        public static readonly Store Instance;
        static Store()
        {
            var products = new List<Product>();

            for (int i = 1; i <= 12; i++)
            {
                products.Add(new Product
                {
                    id = i,
                    Name = $"Product {i}",
                    ImageUrl = $"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
                    Price = 10.00m * i,
                    Description = $"Description of Product {i}"
                });
            }

            Instance = new Store(products, 13);
        }

        public Sale Purchase(Cart cart)
        {
            if (cart.ProductIds.Count == 0)
                throw new ArgumentException("Cart must contain at least one product.");
            if (string.IsNullOrWhiteSpace(cart.Address))
                throw new ArgumentException("Address must be provided.");

            IEnumerable<Product> matchingProducts = Products.Where(p => cart.ProductIds.Contains(p.id.ToString())).ToList();

            IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                product.Price *= (1 + (decimal)TaxPercentage / 100);
                purchaseAmount += product.Price;
            }

            PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);

            var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, paymentMethod);

            return sale;
        }
    }
}
