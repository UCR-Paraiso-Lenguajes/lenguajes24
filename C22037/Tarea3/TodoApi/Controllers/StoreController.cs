using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Business;

namespace TodoApi.Models
{
    [Route("api/")]
    [ApiController]
    public class storeController : ControllerBase
    {
        [HttpGet("store")]
        public async Task<IActionResult> GetStoreAsync()
        {
            var store = await Store.Instance;
            var categories = new Categories().GetCategories();
            return Ok(new {store, categories});
        }

        [HttpGet("store/products")]
        public async Task<IActionResult> GetProductsByCategoryAsync(int category)
        {
            if (category <= 0)return BadRequest("Category ID must be greater than 0.");
            var store = await Store.Instance;
            var products = store.GetProductsByCategory(category);
            return Ok(new{products});
        }
    }
}