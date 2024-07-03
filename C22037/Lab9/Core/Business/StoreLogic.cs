using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.Database;
using TodoApi;

namespace TodoApi.Business
{
    public sealed class StoreLogic
    {
        private SaleDB saleDB = new SaleDB();

        public Sale Purchase(Cart cart)
        {
            if (cart.ProductIds.Count == 0) throw new ArgumentException("Cart must contain at least one product.");
            if (string.IsNullOrWhiteSpace(cart.Address)) throw new ArgumentException("Address must be provided.");

            var products = Store.Instance.Products;
            var taxPercentage = Store.Instance.TaxPercentage;

            // Find matching products based on the product IDs in the cart
            IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.Id.ToString())).ToList();

            // Create shadow copies of the matching products
            IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            // Calculate purchase amount by multiplying each product's price with the store's tax percentage
            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                product.Price *= (1 + (decimal)taxPercentage / 100);
                purchaseAmount += product.Price;
            }

            string purchaseNumber = GenerateNextPurchaseNumber();

            PaymentMethod.Type paymentMethodType = cart.PaymentMethod;

            var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, paymentMethodType);
            
            saleDB.Save(sale);
                
            return sale;
        }

        public static string GenerateNextPurchaseNumber()
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomLetters = new string(Enumerable.Repeat(chars, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            int randomNumber = random.Next(100000, 999999);

            string purchaseNumber = $"{randomLetters}-{randomNumber}";

            return purchaseNumber;
        }
    }
}