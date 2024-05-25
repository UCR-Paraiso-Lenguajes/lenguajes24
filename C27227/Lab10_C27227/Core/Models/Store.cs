using KEStoreApi.Data;

namespace KEStoreApi
{
    public sealed class Store
    {
        public IEnumerable<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }

        private Store(IEnumerable<Product> products, int taxPercentage)
        {
            if (products == null || !products.Any())
            {
                throw new ArgumentException("La lista de productos no puede ser nula ni estar vacía.", nameof(products));
            }

            if (taxPercentage < 0 || taxPercentage > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(taxPercentage), "El porcentaje de impuestos debe estar en el rango de 0 a 100.");
            }

            this.Products = products;
            this.TaxPercentage = taxPercentage;
        }

        public static async Task<Store> InitializeInstance()
        {
            var products = await DatabaseStore.GetProductsFromDBaAsync();
            return new Store(products, 13);
        }


        private static readonly Lazy<Task<Store>> InstanceTask = new Lazy<Task<Store>>(InitializeInstance);
        public static  Task<Store> Instance =>  InstanceTask.Value;

    }
} 
