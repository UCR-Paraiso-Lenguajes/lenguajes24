using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using storeapi.Models;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly Payment _paymentInstance;

        public PaymentController()
        {
            _paymentInstance = Payment.Instance;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPayment()
        {
            if (_paymentInstance == null)
            {
                return StatusCode(500, "La instancia de Payment no está disponible.");
            }

            return Ok(_paymentInstance);
        }
    }
}
