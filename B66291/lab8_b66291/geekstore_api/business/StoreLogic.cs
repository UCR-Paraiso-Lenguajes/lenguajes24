using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using geekstore_api;

public sealed class StoreLogic
{
    public StoreLogic()
    {
    }

    public Sale Purchase (Cart cart)
    {
        if (cart.ProductIds.Count == 0)  throw new ArgumentException("Cart must contain at least one product.");
        if (string.IsNullOrWhiteSpace(cart.Address))throw new ArgumentException("Address must be provided.");

        var products = Store.Instance.Products;
        var taxPercentage = Store.Instance.TaxPercentage;

         // Find matching products based on the product IDs in the cart
        IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.id.ToString())).ToList();

        // Create shadow copies of the matching products
        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

        // Calculate purchase amount by multiplying each product's price with the store's tax percentage
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.price *= (1 + (decimal)taxPercentage / 100);
            purchaseAmount += product.price;
        }

        PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);

        // Create a sale object
        var sale = new Sale( shadowCopyProducts, cart.Address, purchaseAmount, paymentMethod);
        return sale;

    }
}