using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Queries;

namespace StoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ReportsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReportsController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            this.mediator = mediator;
        }

        [HttpGet("Date"), Authorize(Roles = "Admin")]
        public async Task<Reports> GetReportsByDateAsync(DateTime dateTime)
        {
            var dailySalesTask = mediator.Send(new GetDailySalesQuery() { DateTime = dateTime });
            var weeklySalesTask = mediator.Send(new GetWeeklySalesByDateQuery() { DateTime = dateTime });

            await Task.WhenAll(dailySalesTask, weeklySalesTask);

            var dailySales = await dailySalesTask;
            var weeklySales = await weeklySalesTask;

            return new Reports(dailySales, weeklySales);
        }
    }
}



