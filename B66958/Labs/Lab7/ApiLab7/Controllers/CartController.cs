using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateCartAsync([FromBody] Cart cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            CartBusiness cartBusiness = new CartBusiness();
            Sale sale = await cartBusiness.PurchaseAsync(cart);
            return Ok(new { purchaseNumber = sale.PurchaseNumber });
        }
    }
}
