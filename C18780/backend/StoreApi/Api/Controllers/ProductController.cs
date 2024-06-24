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
    public sealed class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        private CategoriesCache categoriesCache;
        private ProductsCache productsCache = ProductsCache.GetInstance();
        public ProductController(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentException("Illegal action, the mediator is being touched. The mediator is null and void.");
            }
            this.mediator = mediator;
            categoriesCache = CategoriesCache.GetInstance();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<Product>> GetProductListAsync()
        {
            var product = await mediator.Send(new GetProductListQuery());

            return product;
        }

        [HttpGet("productId")]
        [AllowAnonymous]
        public async Task<Product> GetProductByIdAsync(Guid uuid)
        {
            var product = await mediator.Send(new GetProductByIdQuery() { Uuid = uuid });

            return product;
        }

        [HttpGet("productCategory")]
        [AllowAnonymous]
        public async Task<List<Product>> GetProductByCategoryAsync(string categoryName)
        {
            var guidCategory = categoriesCache.GetCategoryByName(categoryName);
            var product = await mediator.Send(new GetProductByCategoryQuery() { Category = guidCategory.Uuid });

            return product;
        }

        [HttpPost, Authorize(Roles = "Operator")]
        public async Task<Product> AddProductAsync([FromBody] Product products)
        {
            var product = await mediator.Send(new CreateProductCommand(
                products.Name,
                products.ImageUrl,
                products.Price,
                products.Description,
                products.Category
                ));
            productsCache.setOneProduct(product);
            return product;
        }

        [HttpPut, Authorize(Roles = "Operator")]
        public async Task<int> UpdateProductAsync([FromBody] Product products)
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

        [HttpDelete, Authorize(Roles = "Operator")]
        public async Task<int> DeleteProductAsync(Guid Uuid)
        {
            return await mediator.Send(new DeleteProductCommand() { Uuid = Uuid });
        }
    }
}