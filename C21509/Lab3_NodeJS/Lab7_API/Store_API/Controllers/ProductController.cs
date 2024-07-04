using Core.Business;
using Microsoft.AspNetCore.Mvc;
using Store_API.Models;
using Core.Models;
using Store_API.Database;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductLogic productLogic;
        private readonly DB_API dbApi;

        public ProductController(DB_API dbApi)
        {
            this.dbApi = dbApi ?? throw new ArgumentNullException(nameof(dbApi));
            productLogic = new ProductLogic();
            productLogic.referenceProductInserted = Store.Instance.AddNewProductToStore;
        }

        [HttpPost("product/insert")]
        public async Task<IActionResult> InsertProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("The product object cannot be null.");
            }

            var insertedProduct = new Product(
                product.Name,
                product.ImageURL,
                product.Price,
                product.Description,
                product.Id,
                new Category(product.Categoria.IdCategory, product.Categoria.NameCategory)
            );

            try
            {
                ProductLogic productLogic= new ProductLogic();
                bool insertedProductStatus = await productLogic.insertProductAsync(insertedProduct, Store.Instance.AddNewProductToStore);
                return Ok(insertedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(new { insertedProductStatus = false, message = ex.Message });
            }
        }
        [HttpPost("product/delete")]
        //[HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductInDBAsync([FromBody] Product productToDelete)
        {
            try
            {
                bool deleteProductStatus = await productLogic.deleteProductAsync(productToDelete.Id);
                return Ok(new { deleteProductStatus });
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, $"Product cannot be null: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error has occurred: {ex.Message}");
            }
        }
    }
}