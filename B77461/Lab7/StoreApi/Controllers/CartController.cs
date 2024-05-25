using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using System;
using System.Collections.Generic;
using StoreApi.Handlers;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static List<Cart> Carts = new List<Cart>();
        private readonly CartHandler _cartHandler;

        public CartController()
        {
            _cartHandler = new CartHandler();
        }

        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
         //   Carts.Add(cart);

           // return Ok(cart);

            Sale sale = _cartHandler.Purchase(cart);

            return Ok(sale);
        }
    }

}