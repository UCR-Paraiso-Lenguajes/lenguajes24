using Microsoft.AspNetCore.Mvc;
using System;
using storeapi.Models;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPayment()
        {
            var paymentInstance = Payment.Instance;

            if (paymentInstance == null)
            {
                return StatusCode(500, "La instancia de Payment no está disponible.");
            }

            return Ok(paymentInstance);
        }
    }
}
