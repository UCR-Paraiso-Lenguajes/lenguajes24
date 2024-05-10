using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using storeapi.Database;
using storeapi.Models;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        [HttpGet("GetProducts")]
        public IActionResult GetProducts([FromQuery] string categoryID)
        {
            if (string.IsNullOrWhiteSpace(categoryID))
            {
                return BadRequest("La categoría no puede estar vacía.");
            }

            if (!int.TryParse(categoryID, out int idCategoryParsed))
            {
                return BadRequest("La categoría debe ser un número entero.");
            }

            Products products = new Products();
            return Ok(products.LoadProductsFromDatabase(idCategoryParsed));
        }
    }
}
