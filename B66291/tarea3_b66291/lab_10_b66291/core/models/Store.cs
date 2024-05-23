using core.DataBase;
namespace core.Models
{
    public sealed class Store
    {
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }

        private Store(List<Product> products, int taxPercentage){
        if (products == null)
        {
            throw new ArgumentNullException(nameof(products), "Lista de productos no puede ser nula");
        }
        if (taxPercentage < 0 || taxPercentage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(taxPercentage), "El porcentaje de impuestos debe estar entre 0 y 100");
        }
        this.Products = products;
        this.TaxPercentage = taxPercentage;
        }

        public static readonly Store Instance;

        static Store()
        {
            var products = StoreDb.ExtraerProductosDB(); 
            Instance = new Store(products, 13);
        }

        public void SetProducts(List<Product> newProducts){
            if (newProducts == null)
            {
                throw new ArgumentNullException(nameof(newProducts), "La lista de productos no puede ser nula");
            }
                Instance.Products = newProducts;
            }
        }
}