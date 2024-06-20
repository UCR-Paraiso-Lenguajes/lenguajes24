using System;
using System.Collections.Generic;
using TodoApi.Business;
using TodoApi.Database;

namespace TodoApi.Models
{
    public sealed class Sale
    {
        public IEnumerable<Product> Products { get; private set; }
        public string Address { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMethod.Type PaymentMethod { get; private set; }
        public string PurchaseNumber { get; private set; }

        public Sale(IEnumerable<Product> products, string address, decimal amount, PaymentMethod.Type paymentMethod)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products), "Products cannot be null.");
            if (address == null) throw new ArgumentNullException(nameof(address), "Address cannot be null.");
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
            if (paymentMethod == null) throw new ArgumentNullException(nameof(paymentMethod), "Payment method cannot be null.");

            Products = products;
            Address = address;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PurchaseNumber = StoreLogic.GenerateNextPurchaseNumber();
        }
    }
}