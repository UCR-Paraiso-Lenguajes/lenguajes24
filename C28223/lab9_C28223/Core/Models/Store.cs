using MySqlConnector;
using storeApi.DataBase;

namespace storeApi
{
    public sealed class Store
    {
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }

        private Store(List<Product> products, int TaxPercentage)
        {
            this.Products = products;
            this.TaxPercentage = TaxPercentage;
        }

        public readonly static Store Instance;
        // Static constructor
        static Store()
        {
            var products = StoreDataBase.GetProductsFromDB();
            Store.Instance = new Store(products, 13);
        }

    }
}