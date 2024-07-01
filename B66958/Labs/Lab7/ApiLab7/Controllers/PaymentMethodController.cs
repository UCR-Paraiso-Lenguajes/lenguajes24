using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        [HttpPut("enable"), Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> EnablePaymentMethodAsync([FromBody] PaymentMethodRequest request)
        {
            await Store.Instance.EnablePaymentMethodAsync(request.PaymentMethodType);
            return Ok();
        }

        [HttpPut("disable"), Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> DisablePaymentMethodAsync([FromBody] PaymentMethodRequest request)
        {
            await Store.Instance.DisablePaymentMethodAsync(request.PaymentMethodType);
            return Ok();
        }
    }

    public class PaymentMethodRequest
    {
        public PaymentMethods.Type PaymentMethodType { get; set; }
    }
}