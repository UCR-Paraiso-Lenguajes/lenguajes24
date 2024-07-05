using Microsoft.AspNetCore.Mvc;
using storeApi;
using storeApi.db;
using Microsoft.Extensions.Configuration;
using storeApi.Business;
using Microsoft.AspNetCore.Authorization;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private StoreLogic storeLogic;

        public ProductController(IConfiguration configuration)
        {
            storeLogic = new StoreLogic();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (product == null ||
                string.IsNullOrWhiteSpace(product.Name) ||
                string.IsNullOrWhiteSpace(product.Description) ||
                string.IsNullOrWhiteSpace(product.ImageURL) ||
                product.Price <= 0 ||
                product.Category.Id <= 0)
            {
                return BadRequest("Uno o más parámetros son nulos o tienen valores no válidos.FF");
            }

            try
            {
                await storeLogic.RaiseProductAddedEventAsync(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
