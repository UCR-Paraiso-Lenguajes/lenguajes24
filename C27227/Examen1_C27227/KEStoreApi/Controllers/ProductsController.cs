using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using KEStoreApi.Data;
using Core;

namespace KEStoreApi.Controllers
{
    [Route("admin/")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("productos")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAllProducts()
        {
            var store = await Store.Instance;
            var productsWithCategory = await store.GetAllProductsAndCategoriesAsync();
            return Ok(productsWithCategory);
        }

        [HttpPost("producto")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                return BadRequest(new { message = "Product name is required." });
            }

            if (string.IsNullOrEmpty(product.ImageUrl))
            {
                return BadRequest(new { message = "Product image URL is required." });
            }

            if (product.Price <= 0)
            {
                return BadRequest(new { message = "Product price must be greater than zero." });
            }

            if (product.CategoriaId <= 0)
            {
                return BadRequest(new { message = "Valid category ID is required." });
            }

            var store = await Store.Instance;
            await store.AddProduct(product);
            return CreatedAtAction(nameof(PostProduct), new { id = product.Id }, product);
        }

        [HttpDelete("producto/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await DatabaseStore.DeleteProductAsync(id);
                var store = await Store.Instance;
                await store.RefreshProductsList();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
