using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using storeapi.Bussisnes;
using storeapi.Models;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly StoreLogic _storeLogic;

        public CartController()
        {
            _storeLogic = new StoreLogic();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            try
            {
                ValidateCart(cart);

                var sale = await _storeLogic.PurchaseAsync(cart);

                string purchaseNumber = _storeLogic.PurchaseNumber;

                var response = new { purchaseNumberResponse = purchaseNumber };
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private void ValidateCart(Cart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart), "Cart object cannot be null.");
            }

            if (cart.ProductIds == null || cart.ProductIds.Count == 0)
            {
                throw new ArgumentException("Cart must contain at least one product.", nameof(cart.ProductIds));
            }

            if (string.IsNullOrWhiteSpace(cart.Address))
            {
                throw new ArgumentException("Address must be provided.", nameof(cart.Address));
            }
        }
    }
}
