using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using storeApi.Models;
using storeApi.Database;
using storeApi.Business;

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
        public async Task<IActionResult> GetCategories(int category)
        {
            if (category < 1) throw new ArgumentException("Invalid category ID");

            var store = Store.Instance;
            var products = await store.GetFilteredProductsAsync(category);

            return Ok(new { products });
        }

        




    }

}