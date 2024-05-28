using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost, Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductDTO product)
        {
            ProductBusiness productBusiness = new ProductBusiness();
            var createdProduct = await productBusiness.AddProduct(product);
            return Ok(createdProduct);
        }
    }
}