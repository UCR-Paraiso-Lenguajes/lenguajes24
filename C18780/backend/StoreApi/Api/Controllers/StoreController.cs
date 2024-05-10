using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Queries;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class StoreController : ControllerBase
    {
        private readonly IMediator mediator;

        public StoreController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            this.mediator = mediator;
        }

        [HttpGet("category")]
        public async Task<Store> GetStoreAsync(string name)
        {
            var taxPercentage = 13;//nota: guardar el valor en db
            if (name.Equals("All"))
            {
                var product = await mediator.Send(new GetProductListQuery());
                return new Store(product, taxPercentage);
            }
            else
            {
                var guidCategory = await mediator.Send(new GetCategoryByNameQuery() { Name = name });
                var product = await mediator.Send(new GetProductByCategoryQuery() { Category = guidCategory.Uuid });
                return new Store(product, taxPercentage);
            }
        }
    }
}