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


        [HttpGet]
        public IActionResult GetProducts([FromQuery] string categoryID)
        {

            if (string.IsNullOrWhiteSpace(categoryID))
            {
                return BadRequest("La categoría no puede estar vacía.");
            }

            Categories categories = new Categories();
            int idCategoryparsed = categories.GetCategoryId(categoryID);

            Products products = new Products();
            return Ok(products.LoadProductsFromDatabase(idCategoryparsed));
        }
    }
}
