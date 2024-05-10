using System;
using System.Collections.Generic;

namespace storeapi.Models
{
    public  struct Category
    {
        private int Id { get; set; }
        private string Name { get; set; }
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

  public int GetCategoryId(string categoryId)
{
    // Validar que el categoryId no sea nulo o vacío
    if (string.IsNullOrWhiteSpace(categoryId))
    {
        throw new ArgumentNullException(nameof(categoryId), "El Id de la categoría no puede ser nulo o vacío.");
    }

   
    if (int.TryParse(categoryId, out int parsedCategoryId))
    {
  
        foreach (Category category in ListCategories)
        {
            if (category.Id == parsedCategoryId)
            {
                return parsedCategoryId; // Devuelve el Id de la categoría encontrada
            }
        }
    }

    // Si no se puede convertir a entero o no se encuentra la categoría, lanzar una excepción
    throw new ArgumentException($"La categoría con Id '{categoryId}' no fue encontrada o el formato de Id no es válido.");
}
}
}
