using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Queries;

namespace StoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class SalesController : ControllerBase
    {
        private readonly IMediator mediator;

        public SalesController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            this.mediator = mediator;
        }

        [HttpGet("salesPurchaseNumber")]
        [AllowAnonymous]
        public async Task<Sales> GetSalesByPurchaseNumberAsync(string purchaseNumber)
        {
            var sales = await mediator.Send(new GetSalesByPurchaseNumberQuery() { PurchaseNumber = purchaseNumber });

            return sales;
        }
    }
}