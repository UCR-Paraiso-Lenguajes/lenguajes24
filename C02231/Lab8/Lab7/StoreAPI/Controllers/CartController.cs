using Microsoft.AspNetCore.Mvc;
using StoreAPI.Business;
using StoreAPI.models;
using System;
using System.Collections.Generic;

namespace StoreAPI.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private StoreLogic storeLogic = new StoreLogic();

        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            var sale = storeLogic.Purchase(cart);
            var response = new { purchaseNumber = sale.NumberOrder };
            return Ok(response);
        }
    }

}