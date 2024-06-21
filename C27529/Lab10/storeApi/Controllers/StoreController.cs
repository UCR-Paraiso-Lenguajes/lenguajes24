using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeApi;
using Microsoft.AspNetCore.Authorization;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public async Task<Store> GetStoreAsync()
        {
            return await Task.FromResult(Store.Instance);
        }
        [HttpGet("products")]
        [AllowAnonymous]

        public async Task<IActionResult> GetCategories([FromQuery] string? categoriesString, [FromQuery] string? searchText)
        {


            var store = Store.Instance;
            if (string.IsNullOrEmpty(categoriesString) || categoriesString == "0")
            {
                categoriesString = null;
            }

            if (searchText == "@")
            {
                searchText = null;
            }

            if (categoriesString == null && searchText == null)
            {
                return BadRequest("No categories or search text provided.");
            }

            if (categoriesString != null && searchText != null)
            {
                var categoryIds = categoriesString.Split(',').Select(int.Parse).ToList();
                var filteredStore = await store.GetFilteredProductsAsync(categoryIds);
                var filteredProducts = filteredStore.Products.Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) );
                return Ok(new { products = filteredProducts });
            }
            else if (categoriesString != null)
            {
                var categoryIds = categoriesString.Split(',').Select(int.Parse).ToList();
                var filteredStore = await store.GetFilteredProductsAsync(categoryIds);
                return Ok(new { products = filteredStore.Products });
            }
            else if (searchText != null)
            {
                var filteredProducts = store.GetFilteredTextProducts(searchText);
                return Ok(new { products = filteredProducts });
            }
            return BadRequest("Invalid search parameters.");
        }
    }
}
