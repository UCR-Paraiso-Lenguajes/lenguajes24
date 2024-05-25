using System;
using System.Collections.Generic;
using System.Linq;

namespace storeapi
{
    public sealed class StoreLogic
    {
        private CartSave saleDB = new CartSave();
        private static Random random = new Random();

        public Sale Purchase(Cart cart)
        {
            if (cart.ProductIds.Count == 0)
                throw new ArgumentException("Cart must contain at least one product.");

            if (string.IsNullOrWhiteSpace(cart.Address))
                throw new ArgumentException("Address must be provided.");

            var products = Store.Instance.Products;
            var taxPercentage = Store.Instance.TaxPercentage;

            IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.id.ToString())).ToList();

            // Realiza una copia de los productos para modificar sus precios
            List<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

            decimal purchaseAmount = 0;
            foreach (var product in shadowCopyProducts)
            {
                // Aplica el impuesto al precio del producto
                product.Price *= (1 + (decimal)taxPercentage / 100);
                purchaseAmount += product.Price;
            }

            string purchaseNumber = GenerateNextPurchaseNumber();

            PaymentMethods.Type paymentMethodType = cart.PaymentMethod;

            var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, paymentMethodType);
            CartSave cartSave = new CartSave();

            cartSave.SaveSaleAndItemsToDatabase(sale);

            return sale;
        }

        public static string GenerateNextPurchaseNumber()
        {
            long ticks = DateTime.Now.Ticks;
            int randomNumber = random.Next();
            int uniqueNumber = (int)(ticks & 0xFFFFFFFF) ^ randomNumber;
            return uniqueNumber.ToString();
        }
    }
}

