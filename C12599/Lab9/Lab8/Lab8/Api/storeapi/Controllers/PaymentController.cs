using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace storeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public Payment GetPayment()
        {
            return Payment.Instance ;
        }
    }

}