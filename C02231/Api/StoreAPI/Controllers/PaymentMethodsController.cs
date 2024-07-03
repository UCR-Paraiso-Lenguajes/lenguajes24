using Microsoft.AspNetCore.Mvc;
using Core;
using System.Threading.Tasks;
using System;
using StoreAPI;
using StoreAPI.Database;
using StoreAPI.models;
using System.Collections.Generic;



namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly PaymentMethodDB _paymentMethodDB;

        public PaymentMethodsController(PaymentMethodDB paymentMethodDB)
        {
            _paymentMethodDB = paymentMethodDB;
        }

        [HttpGet]
        public async Task<IEnumerable<PayMethod>> GetPaymentMethodsAsync()
        {
            return await _paymentMethodDB.GetActivePaymentMethodsAsync();
        }

        [HttpPost("{id}/disable")]
        public async Task<IActionResult> DisablePaymentMethodAsync(int id)
        {
            var result = await _paymentMethodDB.DisablePaymentMethodAsync(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Failed to disable payment method.");
        }

        [HttpPost("{id}/toggle")]
        public async Task<IActionResult> TogglePaymentMethodAsync(int id, [FromBody] TogglePaymentMethodRequest request)
        {
            var result = await _paymentMethodDB.TogglePaymentMethodAsync(id, request.Active);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Failed to update payment method status.");
        }
    }
    public class TogglePaymentMethodRequest
    {
        public bool Active { get; set; }
    }
}
