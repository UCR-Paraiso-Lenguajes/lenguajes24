using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace storeApi.Business;
public sealed class LogicStoreApi
{
    public LogicStoreApi(){} // Se utiliza en la creacion del purcharse
    public Sale Purchase(Cart cart)
    {
        var productsIsEmpty = cart.ProductIds.Count == 0;
        var addressIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(cart.Address);

        if (productsIsEmpty) throw new ArgumentException("Cart must contain at least one product.");
        if (addressIsNullOrWhiteSpace) throw new ArgumentException("Address must be provided.");

        var products = Store.Instance.Products;
        var taxPercentage = Store.Instance.TaxPercentage;

        IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.id.ToString())).ToList();

        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();
        
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.price *= (1 + taxPercentage / 100); 
            purchaseAmount += product.price;
        }
         PaymentMethods selectedPaymentMethod = PaymentMethods.SetPaymentType(cart.PaymentMethod);
        // Create a sale object
        var sale = new Sale(Sale.GenerateNextPurchaseNumber(), shadowCopyProducts, cart.Address, purchaseAmount, selectedPaymentMethod.PaymentType);
        return sale;
    }
}