using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Models;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        [HttpGet("Products")]
        public IEnumerable<Product> GetProducts()
        {
            return ProductsLogic.Instance.products;
        }

        [HttpPost("Product")]
        public IActionResult PostProduct([FromBody] Product product)
        {
            // ProductDB.insertProduct(product, ProductsLogic.Instance.addProduct(product));
            ProductsLogic.Instance.addProduct(product);
            return Ok();
        }

    }

}