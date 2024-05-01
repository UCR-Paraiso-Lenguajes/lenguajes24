using System;
using System.Collections.Generic;
using System.Linq;
using Store_API.Database;
using Store_API.Models;

namespace Store_API.Business
{
    public sealed class StoreLogic
    {
        private DB_API dbAPI= new DB_API();

        public string Purchase(Cart cart)
        {
            // Verificar los IDs de productos en el carrito
            Console.WriteLine("Product IDs in the cart:");
            foreach (var productId in cart.ProductIds)
            {
                Console.WriteLine(productId);
            }

            var products = Store.Instance.Products;
            var taxPercentage = Store.Instance.TaxPercentage;

            // Encontrar productos basados en los IDs en el carrito
            IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.Id)).ToList();

            decimal purchaseAmount = 0;
            foreach (var product in matchingProducts)
            {
                purchaseAmount += product.Price;
            }

            PaymentMethods.Type paymentMethodType = cart.PaymentMethod;

            var sale = new Sale(matchingProducts, cart.Address, purchaseAmount, paymentMethodType);

            string purchaseNumber = dbAPI.InsertSale(sale);

            return purchaseNumber;
        }
    }
}