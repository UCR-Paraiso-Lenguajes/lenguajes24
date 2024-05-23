using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using core.Models;
using core.DataBase;
using core.Business;

namespace geekstore_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public Store GetStore()
        {
            return Store.Instance;   
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetSales(int idCat)
        {
            if(idCat < 1){
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
    }

}