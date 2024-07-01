using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private PaymentMethodsBusiness paymentMethodsBusiness;

        [HttpPut("enable"), Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> EnablePaymentMethodAsync([FromBody] PaymentMethodRequest request)
        {
            paymentMethodsBusiness = new PaymentMethodsBusiness();
            await paymentMethodsBusiness.EnablePaymentMethodAsync(request.PaymentMethodType);
            return Ok();
        }

        [HttpPut("disable"), Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> DisablePaymentMethodAsync([FromBody] PaymentMethodRequest request)
        {
            paymentMethodsBusiness = new PaymentMethodsBusiness();
            await paymentMethodsBusiness.DisablePaymentMethodAsync(request.PaymentMethodType);
            return Ok();
        }

        [HttpGet, Authorize(Roles = "Admin,Operator")]
        public IEnumerable<PaymentMethods> GetAllPaymentMethods()
        {
            paymentMethodsBusiness = new PaymentMethodsBusiness();
            IEnumerable<PaymentMethods> paymentMethods = paymentMethodsBusiness.GetAllPaymentMethods();
            return paymentMethods;
        }
    }

    public class PaymentMethodRequest
    {
        public PaymentMethods.Type PaymentMethodType { get; set; }
    }
}