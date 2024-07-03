using Microsoft.AspNetCore.Mvc;
using StoreAPI.Business;
using StoreAPI.models;
using StoreAPI.Database;
using System;
using System.Collections.Generic;

namespace StoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly StoreLogic storeLogic;

        public CartController(PaymentMethodDB paymentMethodDB)
        {
            storeLogic = new StoreLogic(paymentMethodDB);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            try
            {
                if (cart == null || cart.ProductIds == null) return BadRequest("The cart cannot be empty.");

                var sale = await storeLogic.PurchaseAsync(cart);

                var response = new { purchaseNumber = sale.NumberOrder };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}