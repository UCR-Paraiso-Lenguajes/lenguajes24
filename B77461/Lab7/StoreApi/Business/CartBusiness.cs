using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using StoreApi.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace StoreApi.Business;


public class CartBusiness
{
    private CartDB cartDB;
    public IEnumerable<Product> Products { get; }

    public CartBusiness(){
        this.cartDB = new CartDB();
    }

    public Sale Purchase(Cart cart)
    {

         // Find matching products based on the product IDs in the cart
        IEnumerable<Product> matchingProducts = Store.Instance.Products.Where(p => cart.ProductIds.Contains(p.Uuid.ToString())).ToList();

        // Create shadow copies of the matching products
        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

        // Calculate purchase amount by multiplying each product's price with the store's tax percentage
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.price *= (1 + (decimal)Store.Instance.TaxPercentage / 100);
            purchaseAmount += product.price;
        }

        PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);
        
        string receiptNumber = GeneratePurchaseNumber();

        var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, paymentMethod, receiptNumber);

        cartDB.save(sale);

        return sale;
    }

    public void ValidateCart(Cart cart)
    {
        if (cart.ProductIds.Count == 0)
            throw new ArgumentException("Cart must contain at least one product.");
        if (string.IsNullOrWhiteSpace(cart.Address))
            throw new ArgumentException("Address must be provided.");
        if (cart.PaymentMethod == null) throw new ArgumentException("A payment method should be provided");
    }

    public void ValidateSale(Sale sale)
    {
        if (sale.Products.Count() == 0) 
            throw new ArgumentException("Sale must contain at least one product.");
        if (string.IsNullOrWhiteSpace(sale.Address)) 
            throw new ArgumentException("Address must be provided.");
        if (sale.Amount <= 0) 
            throw new ArgumentException("The final amount should be above 0");
        if (sale.PaymentMethod == null) 
            throw new ArgumentException("A payment method should be provided");
        if (string.IsNullOrWhiteSpace(sale.PurchaseNumber)) 
            throw new ArgumentException("A purchase number must be provided");
    }

    private string GeneratePurchaseNumber(){
        DateTime now = DateTime.Now;

        string dateTimeString = now.ToString("yyyyMMddHHmmss");

        byte[] hashBytes;
        using (SHA256 sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dateTimeString));
        }

        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        string receipt = sb.ToString().Substring(0, 20);

        return receipt;
    }
}