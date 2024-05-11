using System;
using System.Collections.Generic;
using System.Linq;

namespace storeapi.Models
{
    public struct Category
    {
        internal int Id { get; }
        internal string Name { get; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Categories
    {
        public List<Category> ListCategories { get; } = new List<Category>
        {
            new Category(1, "Electrónica"),
            new Category(2, "Moda"),
            new Category(3, "Hogar y jardín"),
            new Category(4, "Deportes y actividades al aire libre"),
            new Category(5, "Belleza y cuidado personal"),
            new Category(6, "Alimentación y bebidas"),
            new Category(7, "Libros y entretenimiento"),
            new Category(8, "Tecnología"),
            new Category(9, "Deportes")
        };

        public Categories()
        {
            ListCategories.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));
        }

        public Category GetCategoryById(int categoryId)
        {
            return ListCategories.FirstOrDefault(c => c.Id == categoryId);
        }
    }
}

