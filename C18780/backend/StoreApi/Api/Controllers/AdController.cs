using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Cache;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Queries;

namespace StoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AdController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            _mediator = mediator;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Ad>> GetAdListAsync()
        {
            var ad = await _mediator.Send(new GetAdListQuery());

            return ad;
        }

        [HttpPost, Authorize(Roles = "Operator")]
        public async Task<Ad> AddAdAsync([FromBody] string message)
        {
            var ad = await _mediator.Send(new CreateAdCommand(
                message,
                DateTime.Today
            ));

            return ad;
        }

        [HttpDelete, Authorize(Roles = "Operator")]
        public async Task<int> DeleteAdAsync(Guid uuid)
        {
            return await _mediator.Send(new DeleteAdCommand() { Uuid = uuid });
        }
    }
}