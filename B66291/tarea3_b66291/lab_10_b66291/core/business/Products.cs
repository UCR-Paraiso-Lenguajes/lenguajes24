using System.Collections;
using System.Runtime.ExceptionServices;
using core.Models;
using System.Collections.Generic;
using System.Linq;

namespace core.Business
{
    public class Products 
    {
        public Dictionary<int, List<Product>> filter {private set; get;} 

        public Products()
        {
            filter = new Dictionary<int, List<Product>>();
        }

        public void FiltrarProductosCategoria(int idCat, IEnumerable<Product> listaProductos)
        {
            if (idCat < 1)
            {
                throw new ArgumentException("Id de categoría debe ser mayor que cero.");
            }
            if (listaProductos == null)
            {
                throw new ArgumentNullException(nameof(listaProductos), "La lista de productos no puede ser nula.");
            }
            var productsInCategory = listaProductos.Where(p => p.category.id == idCat).ToList();
            filter[idCat] = productsInCategory;
        }

        public List<Product> ObtenerProductosFiltrados(int idCat)
{           if (idCat < 1)
            {
                throw new ArgumentException("Id de categoría debe ser mayor que cero.");
            }
            if (!filter.ContainsKey(idCat))
            {
                throw new KeyNotFoundException("No se encontró la categoría con el id especificado.");
            }
            List<Product> filteredProducts = filter[idCat].OrderBy(p => p.name).ToList();
            return filteredProducts;
        }
    }
}