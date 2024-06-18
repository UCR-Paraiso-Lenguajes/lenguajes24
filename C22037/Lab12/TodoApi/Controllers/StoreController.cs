using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Business;

namespace TodoApi.Models
{
    [Route("api/")]
    [ApiController]
    public class storeController : ControllerBase
    {
        private static Store storeInstance;

        [HttpGet("store")]
        public async Task<IActionResult> GetStoreAsync()
        {
            storeInstance = await Store.InstanceAsync();
            var categories = new Categories().GetCategories();
            return Ok(new { store = storeInstance, categories });
        }

        [HttpGet("store/categories")]
        public IActionResult GetCategories()
        {
            var categories = new Categories().GetCategories();
            return Ok(new { categories });
        }

        [HttpGet("store/products")]
        public async Task<IActionResult> GetProductsByCategoryAsync([FromQuery] string categories)
        {
            var store = await Store.InstanceAsync();
            IEnumerable<Product> products;

            if (string.IsNullOrEmpty(categories) || categories.ToLower() == "null")
            {
                products = store.Products;
            }
            else
            {
                var categoryIds = categories.Split(',').Select(int.Parse).ToList();
                var productStores = await Task.WhenAll(categoryIds.Select(id => store.GetProductsByCategoryAsync(id)));
                products = productStores.SelectMany(s => s.Products);
            }

            return Ok(new { products });
        }

        [HttpGet("store/search")]
        public async Task<IActionResult> SearchAsync([FromQuery] string search, [FromQuery] string categories)
        {
            var store = await Store.InstanceAsync();
            var categoryLogic = new CategoryLogic(new Categories().GetCategories(), store.Products);

            IEnumerable<Product> products;

            if (string.IsNullOrEmpty(categories) || categories.ToLower() == "null")
            {
                products = await categoryLogic.GetProductsBySearchAsync(search);
            }
            else
            {
                products = await categoryLogic.GetProductsBySearchAsync(search, categories);
            }

            return Ok(new { products });
        }
    }
}