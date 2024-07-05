using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeApi.Models;
using storeApi.Database;
using System.Text.RegularExpressions;
using storeApi.db;

namespace storeApi.Business
{
    public sealed class StoreLogic
    {


        public delegate void ProductAddedHandler(Product product);

        ProductAddedHandler OnProductAdded = (product) =>
        {
            var store = Store.Instance;// instancia de la tienda para anadir el nuevo producto
            store.AddProductToStore(product);
        };

        // public static event ProductAddedHandler OnProductAdded;
        private SaleDB saleDB = new SaleDB();

        public StoreLogic()
        {

        }

        public async Task<Sale> PurchaseAsync(Cart cart) //UT
        {
            if (cart.ProductIds.Count == 0) throw new ArgumentException("Cart must contain at least one product.");
            if (string.IsNullOrWhiteSpace(cart.Address)) throw new ArgumentException("Address must be provided.");
            if (!IsValidAddress(cart.Address)) throw new ArgumentException("Invalid delivery address.H");

            var products = Store.Instance.Products;

            // Find matching products based on the product IDs in the cart
            IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.Id.ToString())).ToList();

            // Create shadow copies of the matching products
            IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            string purchaseNumber = GenerateNextPurchaseNumber();

            PaymentMethod.Type paymentMethodType = cart.PaymentMethod;

            var sale = new Sale(shadowCopyProducts, cart.Address, cart.Total, paymentMethodType);

            await saleDB.SaveAsync(sale);

            return sale;
        }

        private bool IsValidAddress(string address)
        {
            return address.Length > 5;
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

        public async Task RaiseProductAddedEventAsync(Product product)
        {
            StoreDB db = new StoreDB();
            await db.AddProductAsync(product, OnProductAdded);
        }
    }
}
