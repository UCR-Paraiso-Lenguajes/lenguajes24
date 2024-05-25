using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using StoreApi.Models;

namespace StoreApi.Handlers;


public class CartHandler
{
    private readonly CartRepository _cartRepository; 
    public List<Product> Products { get; private set; }
    public int TaxPercentage { get; private set; }


    public CartHandler()
    {
        _cartRepository = new CartRepository();
        
    }

    public Sale Purchase(Cart cart)
    {
        ValidateCart(cart);

         // Find matching products based on the product IDs in the cart
        IEnumerable<Product> matchingProducts = Products.Where(p => cart.ProductIds.Contains(p.Uuid.ToString())).ToList();

        // Create shadow copies of the matching products
        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

        // Calculate purchase amount by multiplying each product's price with the store's tax percentage
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.Price *= (1 + (decimal)TaxPercentage / 100);
            purchaseAmount += product.Price;
        }

        PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);

        var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, paymentMethod);

        ValidateSale(sale);

        return sale;

       // return _cartRepository.Purchase(cart);
    }

    private void ValidateCart(Cart cart)
    {
        if (cart.ProductIds.Count == 0)
            throw new ArgumentException("Cart must contain at least one product.");
        if (string.IsNullOrWhiteSpace(cart.Address))
            throw new ArgumentException("Address must be provided.");
        
    }

    private void ValidateSale(Sale sale)
    {
        if (sale.Products.Count() == 0) 
            throw new ArgumentException("Sale must contain at least one product.");
        if (string.IsNullOrWhiteSpace(sale.Address)) 
            throw new ArgumentException("Address must be provided.");
        if (sale.Amount <= 0) 
            throw new ArgumentException("The final amount should be above 0");
        if (sale.PaymentMethod == null) 
            throw new ArgumentException("A payment method should be provided");
    }
}