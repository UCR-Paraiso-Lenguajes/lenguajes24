using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            ValidateCart(cart);

            var sale = _storeLogic.PurchaseAsync(cart);
            var response = new { purchaseNumberResponse = StoreLogic.GenerateNextPurchaseNumber() };
            return Ok(response);
        }

        private void ValidateCart(Cart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException("cart", "Cart object cannot be null.");
            }

            if (cart.ProductIds == null || cart.ProductIds.Count == 0)
            {
                throw new ArgumentException("Cart must contain at least one product.", "ProductIds");
            }

            if (string.IsNullOrWhiteSpace(cart.Address))
            {
                throw new ArgumentException("Address must be provided.", "Address");
            }

            if (cart.TotalPrice <= 0)
            {
                throw new ArgumentException("Total price must be greater than zero.", "TotalPrice");
            }
        }
    }
}

