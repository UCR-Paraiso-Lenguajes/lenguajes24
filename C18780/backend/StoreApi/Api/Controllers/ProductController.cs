using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Queries;

namespace StoreApi
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Product>> GetProductListAsync()
        {
            var product = await mediator.Send(new GetProductListQuery());

            return product;
        }

        [HttpGet("productId")]
        public async Task<Product> GetProductByIdAsync(Guid uuid)
        {
            var product = await mediator.Send(new GetProductByIdQuery() { Uuid = uuid });

            return product;
        }

        [HttpGet("productCategory")]
        public async Task<List<Product>> GetProductByCategoryAsync(string categoryName)
        {
            var guidCategory = await mediator.Send(new GetCategoryByNameQuery() {Name = categoryName});
            var product = await mediator.Send(new GetProductByCategoryQuery() { Category = guidCategory.Uuid });

            return product;
        }

        [HttpPost]
        public async Task<Product> AddProductAsync(Product products)
        {
            var product = await mediator.Send(new CreateProductCommand(
                products.Name,
                products.ImageUrl,
                products.Price,
                products.Description,
                products.Category
                ));
            return product;
        }

        [HttpPut]
        public async Task<int> UpdateProductAsync(Product products)
        {
            var isProductUpdated = await mediator.Send(new UpdateProductCommand(
               products.Uuid,
               products.Name,
               products.ImageUrl,
               products.Price,
               products.Description,
               products.Category));
            return isProductUpdated;
        }

        [HttpDelete]
        public async Task<int> DeleteProductAsync(Guid Uuid)
        {
            return await mediator.Send(new DeleteProductCommand() { Uuid = Uuid });
        }
    }
}