// File: storeapi/Business/InsertProductsLogic.cs
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
        private readonly bool _shouldSaveToDatabase;

        public InsertProductsLogic(IMemoryCache cache, bool shouldSaveToDatabase = true)
        {
            _cache = cache;
            _shouldSaveToDatabase = shouldSaveToDatabase;
        }

        public List<Product> InsertProduct(Product product)
        {
            ValidateProduct(product);

            if (!_cache.TryGetValue("Products", out List<Product> products))
            {
                products = Store.LoadProductsFromDatabase().ToList();
                _cache.Set("Products", products);
            }

            var existingProduct = products.FirstOrDefault(p => p.id == product.id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
            }
            else
            {
                products.Add(product);
            }

            _cache.Set("Products", products);

            if (_shouldSaveToDatabase)
            {
                StoreDB.InsertProducts(products);
            }

            return products;
        }

        private void ValidateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name must not be null or empty.", nameof(product.Name));
            }

            if (string.IsNullOrWhiteSpace(product.ImageUrl))
            {
                throw new ArgumentException("Product image URL must not be null or empty.", nameof(product.ImageUrl));
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be a positive value.", nameof(product.Price));
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                throw new ArgumentException("Product description must not be null or empty.", nameof(product.Description));
            }


            if (product.Category.Id <= 0)
            {
                throw new ArgumentException("Product category ID must be greater than zero.", nameof(product.Category.Id));
            }

            if (string.IsNullOrWhiteSpace(product.Category.Name))
            {
                throw new ArgumentException("Product category name must not be null or empty.", nameof(product.Category.Name));
            }
        }
    }
}
