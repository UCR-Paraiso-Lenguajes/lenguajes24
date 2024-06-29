using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRWebpack.Hubs;
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
        private readonly IHubContext<CampaignHub> _hubContext;

        public AdController(IMediator mediator, IHubContext<CampaignHub> hubContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Ad>> GetAdListAsync()
        {
            var ads = await _mediator.Send(new GetAdListQuery());

            return ads;
        }

        [HttpPost("latest")]
        [AllowAnonymous]
        public async Task<IEnumerable<Ad>> GetLatestAdsAsync(int count)
        {
            var ads = await _mediator.Send(new GetLatestAdsQuery() { Count = count });

            return ads;
        }

        [HttpPost, Authorize(Roles = "Operator")]
        public async Task<Ad> AddAdAsync([FromBody] string message)
        {
            var ad = await _mediator.Send(new CreateAdCommand(
                message,
                DateTime.Today
            ));

            await _hubContext.Clients.All.SendAsync("AdCreated", ad);

            return ad;
        }

        [HttpDelete, Authorize(Roles = "Operator")]
        public async Task<int> DeleteAdAsync(Guid uuid)
        {
            return await _mediator.Send(new DeleteAdCommand() { Uuid = uuid });
        }
    }
}