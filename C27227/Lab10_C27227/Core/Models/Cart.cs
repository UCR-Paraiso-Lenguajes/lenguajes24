using System;
using System.Collections.Generic;
using static KEStoreApi.Product;

namespace KEStoreApi
{
    public sealed class Cart
    {
         public List<ProductQuantity> Product { get; set; }
        public string address { get; set; }
        public PaymentMethods.Type PaymentMethod { get; set; }
    }
}
