
using MySqlConnector;
using System;
using storeApi.Models;
using System.Net.Http.Headers;
using storeApi.db;

using System.Collections.Generic;
namespace storeApi
{

    public enum ProductCategory
    {
        Audifonos,
        Controles,
        Consolas,
        Videojuegos,
        Mouse,
        Sillas,
        Laptops,
        RealidadVirtual,
        Teclados,
        Monitores,
        Camaras,
        Smartwatches,
        Bicicletas,
        RobotsAspiradores,
        Proyectores,
        Cafeteras
    }




    public sealed class Store
    {
        public IEnumerable<Product> Products { get; private set; } 
        public int TaxPercentage { get; private set; }

        private Store(IEnumerable<Product> products, int taxPercentage) 
        {
            this.Products = products;
            this.TaxPercentage = taxPercentage;
        }

        public readonly static Store Instance;

        // Static constructor
        static Store()
        {
            IEnumerable<Product> products = productsFromDB();
            Store.Instance = new Store(products, 13);
        }

        private static IEnumerable<Product> productsFromDB()
        {
            return StoreDB.GetProducts(); 
        }
    }
}
