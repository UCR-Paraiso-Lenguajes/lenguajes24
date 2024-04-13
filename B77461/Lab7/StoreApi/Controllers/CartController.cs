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

            cartBusiness.ValidateCart(cart);

            Sale sale = cartBusiness.Purchase(cart);

            cartBusiness.ValidateSale(sale);

            return Ok(new {
                purchaseNumber = sale.PurchaseNumber
            });
        }
    }

}