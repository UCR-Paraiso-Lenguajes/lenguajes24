using System;
using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi
{
    public sealed class Cart
    {
        public List<string> ProductIds { get; private set; }
        public string Address { get; private set; }
        public PaymentMethod.Type PaymentMethod { get; private set; }
        public decimal Total { get; private set; }
        public Dictionary<string, int> ProductQuantities { get; private set; }
        
        public Cart(List<string> productIds, string address, PaymentMethod.Type paymentMethod, decimal total, Dictionary<string, int> productQuantities)
        {
            if (productIds == null || productIds.Count == 0) throw new ArgumentNullException(nameof(productIds), "ProductIds cannot be null or empty.");
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
            if (total < 0) throw new ArgumentOutOfRangeException(nameof(total), "Total must be positive.");
            if (productQuantities == null || productQuantities.Count == 0) throw new ArgumentNullException(nameof(productQuantities), "ProductQuantities cannot be null or empty.");

            ProductIds = productIds;
            Address = address;
            PaymentMethod = paymentMethod;
            Total = total;
            ProductQuantities = productQuantities;
        }

        public int GetQuantityForProduct(string productId)
        {
            return ProductQuantities.ContainsKey(productId) ? ProductQuantities[productId] : 0;
        }
    }
}