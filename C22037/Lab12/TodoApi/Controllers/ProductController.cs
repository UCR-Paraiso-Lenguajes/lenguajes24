using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TodoApi.Models;
using TodoApi.Database;
using TodoApi.Business;
using System.Threading.Tasks;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private StoreDB _storeDB = new StoreDB();
        private static Store storeInstance;

        [HttpPost("store/add")]
        public async Task<IActionResult> AddProduct([FromBody] ProductAdd product)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                await _storeDB.InsertProductAsync(product);
                var store = await Store.InstanceAsync();
                IEnumerable<Product> products;
                products = store.Products;
                return Ok(new { products });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("store")]
        public async Task<IActionResult> GetStoreAsync()
        {
            storeInstance = await Store.InstanceAsync();
            var categories = new Categories().GetCategories();
            return Ok(new { store = storeInstance, categories });
        }

        [HttpDelete("store/products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var store = await Store.InstanceAsync();
                if (store == null)
                {
                    return NotFound("Store instance could not be found.");
                }

                var product = store.Products.Find(p => p.Id == id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                await store.RemoveProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}