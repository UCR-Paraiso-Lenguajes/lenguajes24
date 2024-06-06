using Microsoft.AspNetCore.Mvc;
using storeApi.Business;
using storeApi.Models;

using System;
using System.Collections.Generic;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private LogicStoreApi logicStore = new LogicStoreApi();

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            try
            {
                var sale = await logicStore.PurchaseAsync(cart); 
                var numeroCompra = sale.PurchaseNumber;
                var response = new { numeroCompra = numeroCompra };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}