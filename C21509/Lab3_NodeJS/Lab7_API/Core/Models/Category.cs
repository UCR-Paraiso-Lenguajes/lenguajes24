using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public struct Category
    {
        public string NameCategory { get; }
        public int IdCategory { get; }

        public Category(int idCategory, string nameCategory)
        {
            if (idCategory < 1)
            {
                throw new ArgumentException($"Invalid category ID: {idCategory}");
            }

            if (string.IsNullOrWhiteSpace(nameCategory))
            {
                throw new ArgumentException($"Category name cannot be null or empty.");
            }

            IdCategory = idCategory;
            NameCategory = nameCategory;
        }
    }

    public class Categories
    {
        private readonly List<Category> categories = new List<Category>
        {
            new Category(1, "Electrónica"),
            new Category(2, "Hogar y oficina"),
            new Category(3, "Entretenimiento"),
            new Category(4, "Tecnología"),
        };

        public static Categories Instance { get; } = new Categories();
        private Categories() { }

        public static Category GetCategoryById(int categoryId)
        {
            var category = Instance.categories.FirstOrDefault(category => category.IdCategory == categoryId);
            if (category.IdCategory == 0)
            {
                throw new InvalidOperationException($"No category found with ID: {categoryId}");
            }
            return category;
        }

        public static IEnumerable<Category> GetCategories()
        {
            return Instance.categories.OrderBy(category => category.NameCategory);
        }
    }
}