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

        public Cart(List<string> productIds, string address, PaymentMethod.Type paymentMethod, decimal total)
        {
            if (productIds == null) throw new ArgumentNullException("ProductIds cannot be null.");
            if (address == null) throw new ArgumentNullException("Address cannot be null.");
            if (total < 0) throw new ArgumentOutOfRangeException("Total must be positive.");

            ProductIds = productIds;
            Address = address;
            PaymentMethod = paymentMethod;
            Total = total;
        }
    }
}