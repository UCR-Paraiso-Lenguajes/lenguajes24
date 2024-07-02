using Microsoft.AspNetCore.Mvc;
using Store_API.Models;
using Store_API.Business;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private StoreLogic storeLogic = new StoreLogic();

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            if (cart == null)
            {
                return BadRequest("The cart object cannot be null.");
            }

            var actualCart = new Cart(
                cart.Products,
                cart.Address,
                cart.PaymentMethod,
                cart.Total,
                cart.Subtotal
            );

            var successPurchase = await storeLogic.PurchaseAsync(actualCart);
            var response = new { successPurchase };
            return Ok(response);
        }
    }
}