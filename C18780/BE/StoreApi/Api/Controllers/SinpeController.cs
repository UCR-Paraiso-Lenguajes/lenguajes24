using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Commands;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class SinpeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SinpeController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            _mediator = mediator;
        }
        public class SinpeInputModel
        {
            public string purchaseNumber { get; set; }
            public string confirmationNumber { get; set; }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<Sinpe> AddSinpeAsync([FromBody] SinpeInputModel sinpeMovilInput)
        {
            var sales = await GetSalesByPurchaseNumber(sinpeMovilInput.purchaseNumber);
            var sinpeMovil = new Sinpe
            {
                ConfirmationNumber = sinpeMovilInput.confirmationNumber,
                SalesUuid = sales.Uuid
            };

            var createSinpeCommand = new CreateSinpeCommand(
                salesUuid: sinpeMovil.SalesUuid,
                confirmationNumber: sinpeMovil.ConfirmationNumber
            );
            var sinpe = await _mediator.Send(createSinpeCommand);
            return sinpe;
        }

        private async Task<Sales> GetSalesByPurchaseNumber(string purchaseNumber)
        {
            var salesController = new SalesController(_mediator);

            return await salesController.GetSalesByPurchaseNumberAsync(purchaseNumber);
        }
    }

}