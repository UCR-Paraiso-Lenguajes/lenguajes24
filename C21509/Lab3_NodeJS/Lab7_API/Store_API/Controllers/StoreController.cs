using Microsoft.AspNetCore.Mvc;
using Store_API.Models;
using Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public Store GetStore()
        {
            return Store.Instance;
        }

        [HttpGet("Categories")]
        [AllowAnonymous]
        public IEnumerable<Category> GetCategories()
        {
            return Categories.GetCategories();
        }

        [HttpGet("Products")]
        [AllowAnonymous]
        public IEnumerable<Product> GetProductsByCategory([FromQuery] int categoryId)
        {
            if (categoryId < 1)
                throw new ArgumentException($"The {nameof(categoryId)} cannot be less than 1");

            return Store.Instance.Products.Where(p => p.Categoria.IdCategory == categoryId);
        }

        [HttpGet("Search")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetProductByNameAndCategoryIdAsync([FromQuery] string productName, [FromQuery] int categoryId)
        {
            try
            {
                if (string.IsNullOrEmpty(productName) || categoryId < 1)
                    throw new ArgumentException("Product name and category ID are required for search.");

                Console.WriteLine($"Product Name: {productName}, Category ID: {categoryId}");

                var product = await Store.Instance.GetProductByNameAndCategoryIdAsync(productName, categoryId);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}