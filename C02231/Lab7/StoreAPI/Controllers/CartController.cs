using Microsoft.AspNetCore.Mvc;
using StoreAPI;
using System;
using System.Collections.Generic;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static List<Cart> Carts = new List<Cart>();

        //to do generar # compra
        static string GeneratePurchaseNumber()
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 8;

            char[] buffer = new char[length];

            for (int i = 0; i < length; i++)
            {
                buffer[i] = chars[random.Next(chars.Length)];
            }

            return new string(buffer);
        }

        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            // Add the cart to the list
            Carts.Add(cart);
        //private readonly SaleDB sale = new SaleDB();


        //to do: insertar datos

        string purchaseNumber = GeneratePurchaseNumber();
        int paymentMethod = (int)cart.PaymentMethods;
        //sale.save(DateTime.Now, cart.Total, paymentMethod, purchaseNumber);

            // Return the newly created cart
            return Ok(cart);
    }
}

}
//Sale sale = new(1234AB);