using Microsoft.AspNetCore.Mvc;
using KEStoreApi.Bussiness;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace KEStoreApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly StoreLogic _storeLogic;

        public CartController()
        {
            _storeLogic = new StoreLogic();
        }

        [HttpPost("cart")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCartAsync([FromBody] Cart cart)
        {
            if (cart == null)
            {
                return BadRequest("El objeto Cart no puede ser nulo.");
            }

            bool isCartEmpty = cart.Product == null || cart.Product.Count == 0;
            if (isCartEmpty)
            {
                return BadRequest("El carrito debe contener al menos un producto.");
            }

            try
            {
                var sale = await _storeLogic.PurchaseAsync(cart);
                var response = new { purchaseNumber = sale.PurchaseNumber };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
