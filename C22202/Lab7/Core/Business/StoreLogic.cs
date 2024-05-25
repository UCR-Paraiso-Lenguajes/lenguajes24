using System;
using System.Collections.Generic;
using System.Linq;
using ShopApi.Models;

namespace ShopAPI.Business
{
    public sealed class StoreLogic
    {
        private SaleDB saleDB = new SaleDB();

        public Sale Purchase(Cart cart)
        {
            if (cart.productsIds.Count == 0) throw new ArgumentException("Cart must contain at least one product.");
            if (string.IsNullOrWhiteSpace(cart.address)) throw new ArgumentException("Address must be provided.");

            var products = Store.Instance.Products;
            var taxPercentage = Store.Instance.TaxPercentage;

            // Find matching products based on the product IDs in the cart
            IEnumerable<Product> matchingProducts = products.Where(p => cart.productsIds.Contains(p.id.ToString())).ToList();

            // Create shadow copies of the matching products
            IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            // Calculate purchase amount by multiplying each product's price with the store's tax percentage
            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                product.price *= (1 + (decimal)taxPercentage / 100);
                purchaseAmount += product.price;
            }

            string purchaseNumber = GenerateNextPurchaseNumber();
           
            PaymentMethods.Type paymentMethodType = cart.paymentMethod;

            var sale = new Sale(shadowCopyProducts, cart.address, purchaseAmount, paymentMethodType, purchaseNumber);

            saleDB.insertSale(sale);

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