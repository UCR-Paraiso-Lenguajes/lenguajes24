using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using core.Models;
using core.DataBase;
using core.Business;

namespace geekstore_api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly ProductCache productCache = new ProductCache();
        [HttpGet("store")]
        public Store GetStore()
        {
            return Store.Instance;
        }

        [HttpGet("store/products")]
        public async Task<IActionResult> GetSales(int idCat)
        {
            if (idCat < 1)
            {
                throw new ArgumentException("La categoria ingresada es invalida");
            }

            IEnumerable<Product> listaProductos = StoreDb.ExtraerProductosDB();

            Products prod = new Products();
            prod.FiltrarProductosCategoria(idCat, listaProductos);

            List<Product> filteredProductsList = prod.ObtenerProductosFiltrados(idCat);

            var store = Store.Instance;
            store.SetProducts(filteredProductsList);

            return Ok(store);
        }


        [HttpGet("store/products/categories")]
        public async Task<IActionResult> GetCategories(string idSearch, string idSearchCat)
        {
            if (string.IsNullOrWhiteSpace(idSearchCat))
            {
                throw new ArgumentException("La lista de categorías de búsqueda está vacía", nameof(idSearchCat));
            }

            if (string.IsNullOrWhiteSpace(idSearch))
            {
                throw new ArgumentException("El parámetro idSearch está vacío", nameof(idSearch));
            }

            int[] idSearchCatArray = idSearchCat.Split(',').Select(int.Parse).ToArray();

            List<Product> filteredProductsList = productCache.ObtenerProductos(idSearchCatArray.GetHashCode());
            Products prod = new Products();
            if (filteredProductsList == null)
            {
                IEnumerable<Product> listaProductos = StoreDb.ExtraerProductosDB(); ;
                prod.FiltrarProductosBusqueda(idSearchCatArray, listaProductos, idSearch);
                filteredProductsList = prod.ObtenerProductosBusqueda(idSearchCatArray);
                productCache.AgregarProductos(idSearchCatArray.GetHashCode(), filteredProductsList);
            }
            else
            {
                prod.FiltrarProductosBusqueda(idSearchCatArray, filteredProductsList, idSearch);
                filteredProductsList = prod.ObtenerProductosBusqueda(idSearchCatArray);
            }

            var store = Store.Instance;
            store.SetProducts(filteredProductsList);

            return Ok(store);
        }
    }
}