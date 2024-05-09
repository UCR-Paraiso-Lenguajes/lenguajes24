using Microsoft.AspNetCore.Mvc;
using System.Linq;
using storeapi.Database;
using storeapi.Models;
using storeapi.Models;
namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
    
        [HttpGet("GetProducts")]
        public IActionResult GetProducts([FromQuery] string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("La categoría no puede estar vacía.");
            }    
            Categories categories = new Categories();
            string idCategory = categories.GetCategoryIdByName(category);

            Products products = new Products();
            return Ok(products.LoadProductsFromDatabase(idCategory));
        }
    }
}
