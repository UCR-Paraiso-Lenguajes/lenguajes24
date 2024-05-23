using storeApi;
using storeApi.Models;

namespace storeApi
{
    public class Category
    {
        public struct ProductCategory
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public IEnumerable<ProductCategory> GetCategories()
        {
            return SortCategories();
        }

        public IEnumerable<ProductCategory> GetCategoryNames()
        {
            return categories.OrderBy(category => category.Name)
                             .Select(category => new ProductCategory { Id = category.Id, Name = category.Name });
        }


        private IEnumerable<ProductCategory> SortCategories()
        {
            return categories.OrderBy(category => category.Name);
        }

        public ProductCategory GetCategoryById(int categoryId)
        {
            return categories.FirstOrDefault(category => category.Id == categoryId);
        }
        private List<ProductCategory> categories = new List<ProductCategory>
        {
            new ProductCategory { Id = 1, Name = "Audífonos" },
            new ProductCategory { Id = 2, Name = "Controles" },
            new ProductCategory { Id = 3, Name = "Consolas" },
            new ProductCategory { Id = 4, Name = "Videojuegos" },
            new ProductCategory { Id = 5, Name = "Mouse" },
            new ProductCategory { Id = 6, Name = "Sillas" },
            new ProductCategory { Id = 7, Name = "Laptops" },
            new ProductCategory { Id = 8, Name = "RealidadVirtual"},
            new ProductCategory { Id = 9, Name = "Teclados" },
            new ProductCategory { Id = 10, Name = "Monitores" },
            new ProductCategory { Id = 11, Name = "Cámaras" },
            new ProductCategory { Id = 12, Name = "Smartwatches" },
            new ProductCategory { Id = 13, Name = "Bicicletas" },
            new ProductCategory { Id = 14, Name = "RobotsAspiradores" },
            new ProductCategory { Id = 15, Name = "Proyectores" },
            new ProductCategory { Id = 16, Name = "Cafeteras" }
        };




    }
}
