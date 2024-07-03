using StoreApi.Queries;
using StoreApi.utils;
using System.Collections.Generic;

namespace StoreApi.Models
{
    public sealed class Store
    {
        public IEnumerable<Product> Products { get; set; }
        public int TaxPercentage { get; set; }
        public List<PaymentMethods> PaymentMethods { get; set; }

        public Store(IEnumerable<Product> products, List<PaymentMethods> paymentMethods)
        {
            this.Products = products;
            this.PaymentMethods = paymentMethods;
            TaxPercentage = 13;
        }
    }
}
