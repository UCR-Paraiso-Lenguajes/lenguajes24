using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;
namespace StoreApi.Handler
{
    public sealed class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryQuery, List<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByCategoryHandler(IProductRepository productRepository)
        {
            if (productRepository == null)
            {
                throw new ArgumentException("Illegal action, product repository is invalid.");
            }
            _productRepository = productRepository;
        }

        public async Task<List<Product>> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);

            return await _productRepository.GetProductByCategoryAsync(query.Category);
        }

        private void ValidateQuery(GetProductByCategoryQuery query)
        {
            if (query.Category == Guid.Empty)
            {
                throw new ArgumentException("The Category cannot be empty.");
            }
        }
    }
}
