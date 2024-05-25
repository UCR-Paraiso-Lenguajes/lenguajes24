using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TodoApi.Models;
using TodoApi.Database;
using TodoApi.Business;
using System.Threading.Tasks;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cartController : ControllerBase
    {
        private StoreLogic storeLogic = new StoreLogic();

        [HttpPost]
        public async Task<IActionResult> CreateCartAsync([FromBody] Cart cart)
        {
            if (cart == null)
            {
                return BadRequest("Error, cart is null");
            }

            var sale = await storeLogic.PurchaseAsync(cart);

            if (sale == null)
            {
                return BadRequest("Error processing purchase");
            }

            var response = new { purchaseNumberResponse = sale.PurchaseNumber };
            return Ok(response);
        }

    }
}