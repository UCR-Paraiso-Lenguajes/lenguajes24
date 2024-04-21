using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);

            return await _productRepository.GetProductByIdAsync(query.Uuid);
        }
        private void ValidateQuery(GetProductByIdQuery query)
        {
            if (query.Uuid != Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
        }
    }
}
