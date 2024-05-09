using System;
using System.Collections.Generic;

namespace storeapi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Categories
    {
        public List<Category> ListCategories { get; } = new List<Category>
        {
            new Category { Id = 1, Name = "Electrónica" },
            new Category { Id = 2, Name = "Moda" },
            new Category { Id = 3, Name = "Hogar y jardín" },
            new Category { Id = 4, Name = "Deportes y actividades al aire libre" },
            new Category { Id = 5, Name = "Belleza y cuidado personal" },
            new Category { Id = 6, Name = "Alimentación y bebidas" },
            new Category { Id = 7, Name = "Libros y entretenimiento" },
            new Category { Id = 8, Name = "Tecnología" },
            new Category { Id = 9, Name = "Deportes" }
        };

        public Categories()
        {
        
            ListCategories.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));
        }

        public string GetCategoryIdByName(string categoryName)
        {
            // Validar que el nombre de la categoría no sea nulo o vacío
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException(nameof(categoryName), "El nombre de la categoría no puede ser nulo o vacío.");
            }

            // Buscar la categoría por nombre, ignorando mayúsculas/minúsculas
            foreach (Category category in ListCategories)
            {
                if (string.Equals(category.Name, categoryName, StringComparison.OrdinalIgnoreCase))
                {
                    return category.Id.ToString();
                }
            }

            // Si no se encuentra la categoría, lanzar una excepción
            throw new ArgumentException($"La categoría '{categoryName}' no fue encontrada.", nameof(categoryName));
        }
    }
}
