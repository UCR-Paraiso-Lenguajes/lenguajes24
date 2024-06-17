using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using storeapi.Database;
using storeapi.Models;

namespace storeapi.Business
{
    public class InsertProductsLogic
    {
        private readonly IMemoryCache _cache;
     
        private readonly InsertProductDelegate _insertProductDelegate;

        public  delegate void InsertProductDelegate(Product product, List<Product> products);

        public  InsertProductsLogic(IMemoryCache cache, InsertProductDelegate insertProductDelegate)
        {
            _cache = cache;
            _insertProductDelegate = insertProductDelegate;
        }

        public List<Product> InsertProduct(Product product)
        {
            ValidateProduct(product);

            if (!_cache.TryGetValue("Products", out List<Product> products))
            {
                products = Store.LoadProductsFromDatabase().ToList();
                _cache.Set("Products", products);
            }

            _insertProductDelegate(product, products);
            _cache.Set("Products", products);

          
            
                StoreDB.InsertProducts(products);
            

            return products;
        }

        private void ValidateProduct(Product product)
        {
            if (product == null) throw new ArgumentException("El producto no puede ser nulo.");
            if (string.IsNullOrWhiteSpace(product.Name)) throw new ArgumentException("El nombre del producto no puede estar vacío o ser nulo.", nameof(product.Name));
            if (product.Price <= 0) throw new ArgumentOutOfRangeException(nameof(product.Price), "El precio del producto debe ser mayor que cero.");
            if (string.IsNullOrWhiteSpace(product.ImageUrl)) throw new ArgumentException("La URL de la imagen del producto no puede estar vacía o ser nula.", nameof(product.ImageUrl));
            if (string.IsNullOrWhiteSpace(product.Description)) throw new ArgumentException("La descripción del producto no puede estar vacía o ser nula.", nameof(product.Description));
        }
    }
}
