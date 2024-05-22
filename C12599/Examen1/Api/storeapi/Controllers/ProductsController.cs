using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using storeapi.Models;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts([FromQuery] List<string> categoryIDs, string search)
        {
            var categoryIDsParsed = new List<int>();

            foreach (var categoryId in categoryIDs)
            {
                if (!int.TryParse(categoryId, out int categoryIdParsed))
                {
                    return BadRequest("Uno o más IDs de categoría no son números enteros válidos.");
                }
                categoryIDsParsed.Add(categoryIdParsed);
            }

            Products products = new Products();

            IEnumerable<Dictionary<string, string>> filteredProducts = products.LoadProductsFromDatabase(categoryIDsParsed, search);

            return Ok(filteredProducts);
        }
    }
}
