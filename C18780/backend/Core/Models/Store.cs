using StoreApi.Queries;

namespace StoreApi.Models
{
    public sealed class Store
    {
        public IEnumerable<Product> Products { get; set; }
        public int TaxPercentage { get; set; }
        public Store(IEnumerable<Product> products)
        {
            this.Products = products;
            TaxPercentage = 13;
        }
    }
}