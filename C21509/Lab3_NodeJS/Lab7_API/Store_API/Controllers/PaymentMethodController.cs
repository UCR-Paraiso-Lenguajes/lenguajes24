using Microsoft.AspNetCore.Mvc;
using Store_API.Models;
using Store_API.Business;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Store_API.Database;
namespace Store_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly DB_API dbApi;

        public PaymentMethodController()
        {
            dbApi = new DB_API();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {
            try
            {
                var methods = await dbApi.GetPaymentMethodsAsync();
                return Ok(methods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isEnabled)
        {
            try
            {
                await dbApi.UpdatePaymentMethodStatus(id, isEnabled);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}