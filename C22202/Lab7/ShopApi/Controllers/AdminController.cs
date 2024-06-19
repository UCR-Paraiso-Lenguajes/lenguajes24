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
        public IActionResult PutProduct([FromBody] Product product)
        {
            var response = new {purchaseNumber=product};
            return Ok(response);
        }

    }

}