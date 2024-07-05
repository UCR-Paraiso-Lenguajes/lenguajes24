using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using static KEStoreApi.Store;

namespace KEStoreApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet("store")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreAsync()
        {
            var store = await Store.Instance;
            return Ok(new { Products = store.ProductsList, TaxPercentage = store.TaxPercentage, Categorias = store.CategoriasLista });
        }

        [HttpGet("store/products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByCategoryAsync([FromQuery] IEnumerable<int> categoryIds)
        {
            if (categoryIds == null)
            {
                return BadRequest("Los IDs de categoría no pueden ser nulos.");
            }

            try
            {
                var storeInstance = await Store.Instance;
                var productsByCategory = await storeInstance.GetProductosCategoryIDAsync(categoryIds);
                return Ok(new { Products = productsByCategory });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("store/search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProductsAsync([FromQuery] string productName, [FromQuery] IEnumerable<int> categoryIds)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("El nombre del producto no puede estar vacío.");
            }

            try
            {
                var store = await Store.Instance;
                var products = await store.SearchProductsStoreAsync(productName, categoryIds);

                return Ok(new { Products = products });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("store/paymentMethods")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentMethodsAsync()
        {
            var store = await Store.Instance;
            return Ok(store.PaymentMethodsList.Select(pm => new { pm.PaymentType, pm.IsEnabled }));
        }

        [HttpPost("store/paymentMethods/disable")]
        [AllowAnonymous]
        public async Task<IActionResult> DisablePaymentMethodAsync([FromBody] PaymentMethod request)
        {
            if (request == null || request.PaymentType < 0)
            {
                return BadRequest("El tipo de método de pago no puede ser nulo.");
            }

            try
            {
                var store = await Store.Instance;
                var paymentMethod = store.PaymentMethodsList.FirstOrDefault(pm => pm.PaymentType == (PaymentMethods.Type)request.PaymentType);
                if (paymentMethod == null)
                {
                    return NotFound("El método de pago no fue encontrado.");
                }

                paymentMethod.IsEnabled = request.IsEnabled; // Toggle the status based on the request

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
