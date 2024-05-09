using Microsoft.AspNetCore.Mvc;
using storeapi.Models;
using storeapi.Models;
using System;
using System.Collections.Generic;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStore()
        {
            Store storeInstance = Store.Instance;

            if (storeInstance == null)
            {
                throw new InvalidOperationException("No se pudo obtener la instancia de la tienda.");
            }

            Categories categories = new Categories();

            // Crear una lista de objetos para almacenar las categorías
            List<object> categoryList = new List<object>();
            
            foreach (var category in categories.ListCategories)
            {
                categoryList.Add(category);
            }

            var response = new Dictionary<string, IEnumerable<object>>
            {
                ["Products"] = storeInstance.Products,
                ["Categories"] = categoryList  
            };

            return Ok(response);
        }
    }
}
