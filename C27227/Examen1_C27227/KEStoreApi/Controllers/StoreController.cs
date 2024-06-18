using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEStoreApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet("store")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreAsync()
        {
            var store = await Store.Instance;
            return Ok(new { Products = store.ProductsList, TaxPercentage = store.TaxPercentage, Categorias = store.CategoriasLista });
        }

        [HttpGet("store/products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByCategoryAsync([FromQuery] IEnumerable<int> categoryIds)
        {
            if (categoryIds == null)
            {
                return BadRequest("Los IDs de categoría no pueden ser nulos.");
            }

            try
            {
                var storeInstance = await Store.Instance;
                var productsByCategory = await storeInstance.GetProductosCategoryID(categoryIds);
                return Ok(new { Products = productsByCategory });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("store/search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProductsAsync([FromQuery] string productName, [FromQuery] IEnumerable<int> categoryIds)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("El nombre del producto no puede estar vacío.");
            }

            try
            {
                var store = await Store.Instance;
                var products = await store.SearchProductsStore(productName, categoryIds);

                return Ok(new { Products = products });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
