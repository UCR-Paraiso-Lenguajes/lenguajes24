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

        [HttpGet("/active")]
        public async Task<IEnumerable<PayMethod>> GetAllPaymentMethodsAsync()
        {
            return await _paymentMethodDB.GetPaymentMethodsAsync();
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

        [HttpPost("{id}/active")]
        public async Task<IActionResult> TogglePaymentMethodAsync(int id)
        {
            var result = await _paymentMethodDB.ActivePaymentMethodAsync(id);
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
