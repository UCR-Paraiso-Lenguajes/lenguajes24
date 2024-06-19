using Microsoft.AspNetCore.Mvc;
using storeApi;
using storeApi.db;
using Microsoft.Extensions.Configuration;
using storeApi.Business;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreDB _storeDB;

        public ProductController(IConfiguration configuration)
        {
            _storeDB = new StoreDB();
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (product == null ||
                string.IsNullOrWhiteSpace(product.Name) ||
                string.IsNullOrWhiteSpace(product.Description) ||
                string.IsNullOrWhiteSpace(product.ImageURL) ||
                product.Price <= 0 ||
                product.Category.Id <= 0)
            {
                return BadRequest("Uno o más parámetros son nulos o tienen valores no válidos.");
            }

            try
            {
                await _storeDB.AddProductAsync(product);

                return Ok("Product added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
