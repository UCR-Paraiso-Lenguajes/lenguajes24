using Microsoft.AspNetCore.Mvc;
using storeApi.Business;
using storeApi.DataBase;

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
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            var sale = logicStore.Purchase(cart);
            var numeroCompra = sale.PurchaseNumber;
            var response = new { numeroCompra = numeroCompra };
            return Ok(response);
        }
    }

}