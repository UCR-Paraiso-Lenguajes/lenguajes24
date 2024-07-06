using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using storeApi;
using storeApi.db;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var methods = await StoreDB.GetPaymentMethodsAsync();
            return Ok(methods);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentMethod([FromBody] PaymentMethodDTO dto)
        {
            var paymentMethod = PaymentMethod.Find((PaymentMethod.Type)dto.PaymentType);
            paymentMethod.Enabled = dto.Enabled;

            var result = await StoreDB.UpdatePaymentMethodAsync((int)paymentMethod.PaymentType, paymentMethod.Enabled);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Failed to update payment method.");
        }

    }
}
