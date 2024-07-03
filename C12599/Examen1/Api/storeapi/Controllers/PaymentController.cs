using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using storeapi.Models;
using storeapi.Database;
//PROYECTO12
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
        [Route("payment-methods")]
        public IActionResult GetPaymentMethods()
        {
            if (_paymentInstance == null)
            {
                return StatusCode(500, "La instancia de Payment no está disponible.");
            }

            return Ok(new {
                Cash = new { paymentType = PaymentMethods.Type.CASH, isActive = _paymentInstance.Cash.IsActive },
                Sinpe = new { paymentType = PaymentMethods.Type.SINPE, isActive = _paymentInstance.Sinpe.IsActive }
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("update-status")]
        public IActionResult UpdatePaymentStatus([FromBody] UpdatePaymentStatusRequest request)
        {
            if (_paymentInstance == null)
            {
                return StatusCode(500, "La instancia de Payment no está disponible.");
            }

            if (request.Method == null || request.State == null)
            {
                return BadRequest("Invalid request data.");
            }

            PaymentMethods.Type paymentType;
            if (!Enum.TryParse(request.Method.ToUpper(), out paymentType))
            {
                return BadRequest("Método de pago no válido.");
            }

            var paymentMethod = PaymentMethods.Find(paymentType);
            if (paymentMethod == null)
            {
                return BadRequest("Método de pago no encontrado.");
            }

            bool isActive = request.State == "activo";
            paymentMethod.IsActive = isActive;
            PaymentDB.UpdatePaymentMethodStatus((int)paymentType, isActive);

            return Ok(new { message = "Estado del método de pago actualizado con éxito." });
        }
    }

    public class UpdatePaymentStatusRequest
    {
        public string? Method { get; set; }
        public string? State { get; set; }
    }
}
