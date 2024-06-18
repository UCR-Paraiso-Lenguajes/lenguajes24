using Microsoft.AspNetCore.Mvc;
using storeApi.Models.Data;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace storeApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet("store")]
        [AllowAnonymous]

        public async Task<IActionResult> GetStore()
        {
            var store = await Store.Instance;
            return Ok(new
            {
                Products = store.Products,
                TaxPercentage = store.TaxPercentage,
                Categories = store.Categories
            });
        }
        [HttpGet("store/products")]
        [AllowAnonymous]

        public async Task<IActionResult> GetCategoriesAsync([FromQuery(Name = "categoryIDs")] List<int> categoryIDs = null,
            [FromQuery(Name = "searchText")] string searchText = null)//se hace asi ya que puede que reciba algunos parametros, no los dos a la vez, por ejemplo
        {

            var store = await Store.Instance;
            // Validar que al menos uno de los parámetros de búsqueda sea proporcionado
            bool noSearchTextProvided = string.IsNullOrWhiteSpace(searchText);
            bool noCategoryIDsProvided = categoryIDs == null || !categoryIDs.Any();
            if (noSearchTextProvided && noCategoryIDsProvided)
                throw new ArgumentException("Se debe proporcionar al menos un parámetro de búsqueda.");

            bool onlySearchTextProvided = !string.IsNullOrWhiteSpace(searchText) && (categoryIDs == null || !categoryIDs.Any());
            if (onlySearchTextProvided)
            {
                var filteredProducts = await store.getProductByText(searchText);
                return Ok(new { filteredProducts });
            }

            bool onlyCategoryIDsProvided = string.IsNullOrWhiteSpace(searchText) && (categoryIDs != null && categoryIDs.Any());
            if (onlyCategoryIDsProvided)
            {
                foreach (var categoryId in categoryIDs)
                {
                    if (categoryId < 1)
                        throw new ArgumentException($"El ID de la categoría {categoryId} no puede ser negativo o cero.");
                }
                var filteredProducts = await store.getProductosCategoryID(categoryIDs);
                return Ok(new { filteredProducts });
            }
            bool searchTextAndCategoryIDsProvided = !string.IsNullOrWhiteSpace(searchText) && (categoryIDs != null && categoryIDs.Any());
            if (searchTextAndCategoryIDsProvided)
            {
                foreach (var categoryId in categoryIDs)
                {
                    if (categoryId < 1)
                        throw new ArgumentException($"El ID de la categoría {categoryId} no puede ser negativo o cero.");
                }
                var filteredProducts = await store.getProductsCategoryAndText(searchText, categoryIDs);
                return Ok(new { filteredProducts });
            }

            return BadRequest("Los parámetros de búsqueda proporcionados no son válidos.");
        }
    }

}