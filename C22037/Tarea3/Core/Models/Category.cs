using TodoApi;
using TodoApi.Models;
using System;

namespace TodoApi
{
    public class Category
    {
        public CategorySt[] GetCategories()
        {
            SortCategories();
            return categories;
        }

        public struct CategorySt
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private CategorySt[] categories = new CategorySt[]
        {
            new CategorySt { Id = 1, Name = "Food and Beverages" },
            new CategorySt { Id = 2, Name = "Beauty and Personal Care" },
            new CategorySt { Id = 3, Name = "Sports" },
            new CategorySt { Id = 4, Name = "Electronics" },
            new CategorySt { Id = 5, Name = "Home and Garden" },
            new CategorySt { Id = 6, Name = "Technology" }
        };

        public CategorySt GetType(int id)
        {
            if (id <= 0)throw new ArgumentException(nameof(id), "ID must be positive or not 0");

            foreach (var category in categories)
            {
                if (category.Id == id)
                {
                    return category;
                }
            }
            return new CategorySt();
        }

        private void SortCategories()
        {
            Array.Sort(categories, (x, y) => x.Name.CompareTo(y.Name));
        }
    }
}