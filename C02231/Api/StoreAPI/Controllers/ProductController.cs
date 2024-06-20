using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StoreAPI.Business;
using StoreAPI.models;
using System;
using System.Collections.Generic;



namespace StoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductAsync([FromBody] Product product)
        {
            if (product == null ) return BadRequest("The product cannot be null.");
            if (Categories.Instance.GetCategories().All(c => c.IdCategory != product.ProductCategory.IdCategory))
            {
                return BadRequest("Invalid category.");
            }

            await Products.InitializeInstanceAsync();
            ProductLogic productLogic = new ProductLogic();
            var newProduct = await productLogic.AddProduct(product);
            return Ok(newProduct);
        }
    }

}