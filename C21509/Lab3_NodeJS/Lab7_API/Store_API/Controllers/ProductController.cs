using Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_API.Models;
using System;
using System.Threading.Tasks;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductLogic productLogic;

        public ProductController()
        {
            productLogic = new ProductLogic();
            productLogic.referenceProductInserted = Store.Instance.AddNewProductToStore;
        }

        [HttpPost("product/insert")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertNewProductInStoreAsync([FromBody] Product newProduct)
        {
            try
            {
                bool insertedProductStatus = await productLogic.insertProductAsync(newProduct);
                return Ok(new { insertedProductStatus });
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, $"El producto no puede ser nulo: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ha ocurrido un error al agregar el nuevo producto. Por favor inténtalo más tarde. Error: {ex.Message}");
            }
        }

        [HttpPost("product/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductInDBAsync([FromBody] Product productToDelete)
        {
            try
            {
                string deleteProductStatus = "Producto elimina";
                return Ok(new { deleteProductStatus });
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, $"Product cannot be null: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $": {ex.Message}");
            }
        }
    }
}