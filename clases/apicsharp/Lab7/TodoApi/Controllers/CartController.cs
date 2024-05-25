using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TodoApi.business;
using TodoApi.models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private StoreLogic storeLogic = new StoreLogic();
   
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            var cartWitStatus = storeLogic.Purchase(cart);
            if(cartWitStatus is CartApproved)
            {
                var cartApproved = cartWitStatus as CartApproved;
                var sale = cartApproved.Sale;
                var purchaseNumber = sale.PurchaseNumber;

                var response = new {purchaseNumber=purchaseNumber};
                return Ok(response);
            }
            return BadRequest("Pending to approve");
        }
    }

}
