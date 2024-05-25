using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TodoApi.Database;
using TodoApi.Models;


namespace TodoApi.Business{


public sealed class StoreLogic
{

    private SaleDB saleDB = new SaleDB();
    public string Purchase (Cart cart)
    {
        if (cart.ProductIds.Count == 0)  throw new ArgumentException("El carrito debe tener al menos un producto.");
        if (string.IsNullOrWhiteSpace(cart.Address))throw new ArgumentException("Debe ingresar una dirección.");

        var products = Store.Instance.Products;
        var taxPercentage = Store.Instance.TaxPercentage;

       
        IEnumerable<Product> matchingProducts = products.Where(p => cart.ProductIds.Contains(p.id.ToString())).ToList();

       
        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

      
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.price *= (1 + (decimal)taxPercentage / 100);
            purchaseAmount += product.price;
        }

        PaymentMethods paymentMethod = PaymentMethods.SetPaymentType(cart.PaymentMethod);

    
        var venta = new Sale( shadowCopyProducts, cart.Address, purchaseAmount, paymentMethod.PaymentType);

        saleDB.save(venta);

        return venta.PurchaseNumber;

    }

}}