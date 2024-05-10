using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Business
{
    public sealed class CategoryLogic
    {
        private static Dictionary<int, Category.CategorySt> categoryById;
        private static Dictionary<int, List<Product>> productsByCategoryId;

        public CategoryLogic(Category.CategorySt[] categories, IEnumerable<Product> products)
        {
            if (categories == null)throw new ArgumentNullException(nameof(categories), "The list of categories cannot be null.");
            if (products == null)throw new ArgumentNullException(nameof(products), "The list of products cannot be null.");
            if (products.Any(p => p == null))throw new ArgumentException("The list of products cannot contain null elements.", nameof(products));

            categoryById = new Dictionary<int, Category.CategorySt>();
            productsByCategoryId = new Dictionary<int, List<Product>>();

            foreach (var category in categories)
            {
                categoryById.Add(category.Id, category);
                productsByCategoryId.Add(category.Id, new List<Product>());
            }

            foreach (var product in products)
            {
                if (productsByCategoryId.ContainsKey(product.Category.Id))
                {
                    productsByCategoryId[product.Category.Id].Add(product);
                }
            }
        }

        public async Task<IEnumerable<Product>> GetCategoryByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID must be a positive integer.");

            if (productsByCategoryId.ContainsKey(id))
            {
                return productsByCategoryId[id];
            }
            else
            {
                return null;
            }
        }
    }
}