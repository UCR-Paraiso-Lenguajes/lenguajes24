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

        [HttpPut, Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> UpdatePaymentMethodAsync([FromBody] PaymentMethodRequest request)
        {
            if(request == null){
                throw new ArgumentException("A payment method must be provided");
            }
            var requestType = (PaymentMethods.Type) request.PaymentType;
            paymentMethodsBusiness = new PaymentMethodsBusiness();
            if(request.IsEnabled){
                await paymentMethodsBusiness.EnablePaymentMethodAsync(requestType);
            }else{
                await paymentMethodsBusiness.DisablePaymentMethodAsync(requestType);
            }
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

    public class PaymentMethodRequest{
        public int PaymentType { get; set;}
        public bool IsEnabled { get; set; } 
    }
}