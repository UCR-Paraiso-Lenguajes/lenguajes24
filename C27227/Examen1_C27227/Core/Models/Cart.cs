using System;
using System.Collections.Generic;
using KEStoreApi.Models;
using static KEStoreApi.Product;

namespace KEStoreApi
{
    public sealed class Cart
    {
        public List<ProductQuantity> Product { get; set; }
        public Address Address { get; set; }
        public PaymentMethods.Type PaymentMethod { get; set; }
    }
}
