using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class GetProductListHandler : IRequestHandler<GetProductListQuery, List<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductListHandler(IProductRepository productRepository)
        {
            if (productRepository == null)
            {
                throw new ArgumentException("Illegal action, productRepository is invalid.");
            }
            _productRepository = productRepository;
        }

        public async Task<List<Product>> Handle(GetProductListQuery query, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProductListAsync();
        }
    }
}
