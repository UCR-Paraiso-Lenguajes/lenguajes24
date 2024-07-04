using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCartAsync([FromBody] Cart cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!AddressValidator.ValidateAddressFormat(cart.Address))
                return BadRequest("Invalid address format.");
            CartBusiness cartBusiness = new CartBusiness();
            Sale sale = await cartBusiness.PurchaseAsync(cart);
            return Ok(new { purchaseNumber = sale.PurchaseNumber });
        }
    }
}
