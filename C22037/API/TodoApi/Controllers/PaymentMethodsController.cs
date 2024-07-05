using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Database;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly StoreDB _storeDB;

        public PaymentMethodsController(StoreDB storeDB)
        {
            _storeDB = storeDB;
        }

        [HttpPost("updateStatus")]
        public async Task<IActionResult> UpdatePaymentMethodStatus([FromBody] UpdatePaymentMethodStatusRequest request)
        {
            try
            {
                await _storeDB.UpdatePaymentMethodStatusAsync(request.PaymentId, request.IsActive);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getPaymentMethods")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            try
            {
                var methods = await _storeDB.GetPaymentMethodsAsync();
                return Ok(methods);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class UpdatePaymentMethodStatusRequest
    {
        public int PaymentId { get; set; }
        public bool IsActive { get; set; }
    }
}