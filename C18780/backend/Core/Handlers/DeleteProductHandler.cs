using MediatR;
using StoreApi.Commands;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            if (productRepository == null)
            {
                throw new ArgumentException("Illegal action, productRepository is invalid.");
            }
            _productRepository = productRepository;
        }

        public async Task<int> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var product = await _productRepository.GetProductByIdAsync(command.Uuid);
            if (product == null)
                return default;

            return await _productRepository.DeleteProductAsync(product.Uuid);
        }

        private void ValidateCommand(DeleteProductCommand command)
        {
            if (command.Uuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
        }
    }
}