using geekstore_api.DataBase;
using MySqlConnector;
namespace geekstore_api
{

    public sealed class Store
    {
        public List<Product> Products { get; private set; }

        public int TaxPercentage { get; private set; }

        private Store(List<Product> productsL, int taxPercentage){
           this.Products = productsL;
           this.TaxPercentage = taxPercentage;
        } 
        public readonly static Store Instance; 

        static Store()
        {
        var products = StoreDb.ExtraerProductosDB();
        Store.Instance = new Store(products, 13);
        }   
    }
  
}

