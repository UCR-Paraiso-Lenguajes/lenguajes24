using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Business;

namespace TodoApi.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStoreAsync()
        {
            var store = await Store.Instance;
            var categories = new Category().GetCategories();
            return Ok(new {store, categories});
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProductsByCategoryAsync(int category)
        {
            if (category <= 0)return BadRequest("Category ID must be greater than 0.");
            var store = await Store.Instance;
            var products = store.GetProductsByCategory(category);
            return Ok(new{products});
        }
    }
}