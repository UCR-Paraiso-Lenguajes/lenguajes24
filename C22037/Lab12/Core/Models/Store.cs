using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Business;
using TodoApi.Database;

namespace TodoApi.Models
{
    public sealed class Store
    {
        private static Store instance;
        private static readonly object _lock = new object();
        private StoreDB storeDB;
        public List<Product> Products { get; private set; }
        public int TaxPercentage { get; private set; }
        private CategoryLogic categoryLogic;

        public Store(List<Product> products, int taxPercentage, StoreDB db)
        {
            if (products == null) throw new ArgumentNullException(nameof(products), "Products cannot be null.");
            if (!products.Any()) throw new ArgumentException("Products list cannot be empty.", nameof(products));
            if (taxPercentage < 0 || taxPercentage > 100) throw new ArgumentOutOfRangeException(nameof(taxPercentage), "Tax percentage must be between 0 and 100.");

            Products = products;
            TaxPercentage = taxPercentage;
            storeDB = db;
        }

        public static async Task<Store> InstanceAsync()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        var db = new StoreDB();
                        var categories = new Categories().GetCategories();
                        var products = StoreDB.GetProductsAsync().Result.ToList();
                        instance = new Store(products, 13, db);
                        instance.categoryLogic = new CategoryLogic(categories, products);
                    }
                }
            }
            return instance;
        }

        public async Task AddProduct(ProductAdd productAdd, int id)
        {
            var newProduct = new Product(productAdd.name, productAdd.imageUrl, productAdd.price, productAdd.description, id, new Categories().GetType(productAdd.category));
            Products.Add(newProduct);
        }

        public async Task AddProductAsync(ProductAdd product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            await storeDB.InsertProductAsync(product);
        }

        public async Task RemoveProductAsync(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Products.Remove(product);
                await storeDB.DeleteProductAsync(id);
            }
        }

        public async Task<Store> GetProductsByCategoryAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Category ID must be greater than 0.");

            var productsByCategory = await categoryLogic.GetCategoriesByIdAsync(new[] { id });
            return new Store(productsByCategory.ToList(), 13, storeDB);
        }
    }
}