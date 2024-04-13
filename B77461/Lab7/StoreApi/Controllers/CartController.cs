using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using System;
using System.Collections.Generic;
using StoreApi.Business;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            CartBusiness cartBusiness = new CartBusiness();
            Sale sale = cartBusiness.Purchase(cart);
            return Ok(new {
                purchaseNumber = sale.PurchaseNumber
            });
        }
    }

}