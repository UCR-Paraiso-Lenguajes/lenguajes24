using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KEStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStoreAsync()
        {
            var store = await Store.Instance;
            return Ok(new
            {
                Products = store.ProductsList,
                TaxPercentage = store.TaxPercentage,
                Categorias = store.CategoriasLista
            });
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProductsByCategoryAsync(int categoryId)
        {
            if (categoryId == null || categoryId <= 0)
            {
                return BadRequest("El ID de categoría debe ser un número entero mayor que 0.");
            }
            var storeInstance = await Store.Instance;
            var productsByCategory = await storeInstance.getProductosCategoryID(categoryId);
            return Ok(new
            {
                Products = productsByCategory
            });
        }
    }

}