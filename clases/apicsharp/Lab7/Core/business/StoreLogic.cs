using System;
using System.ComponentModel;
using System.Data.Common;
using System.IO.Compression;
using Core;
using MySqlConnector;
using TodoApi.models;

namespace TodoApi.business;
public sealed class StoreLogic
{
    public StoreLogic()
    {


    }
    public CartWithStatus Purchase (Cart cart, out Sale saleArg)
    {  
        if (cart.ProductIds.Count == 0)  throw new ArgumentException("Cart must contain at least one product.");
        if (string.IsNullOrWhiteSpace(cart.Address))throw new ArgumentException("Address must be provided.");

        var products = Store.Instance.Products;
        var taxPercentage = Store.Instance.TaxPercentage;

        // Find matching products based on the product IDs in the cart
        IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.Uuid.ToString())).ToList();

        // Create shadow copies of the matching products
        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

        // Calculate purchase amount by multiplying each product's price with the store's tax percentage
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.Price *= (1 + (decimal)taxPercentage / 100);
            purchaseAmount += product.Price;
        }

        if (purchaseAmount > 1000000) 
        {

            //Broker.message("Avidarle al admin")
            saleArg = null;
            return new CartPendingtoApprove();
        }
        else
        {

            PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);

            // Create a sale object
            var sale = new Sale(Sale.GenerateNextPurchaseNumber(), shadowCopyProducts, cart.Address, purchaseAmount, paymentMethod);
            saleArg = sale;
            return new CartApproved(sale);

        }
    }   
    public CartWithStatus Purchase (Cart cart)
    {
        Sale sale;
        return this.Purchase(cart, out sale);        
    }
}
