using MediatR;
using StoreApi.Commands;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            if (productRepository == null)
            {
                throw new ArgumentException("Illegal action, productRepository is invalid.");
            }
            _productRepository = productRepository;
        }
        public async Task<int> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);
            var product = await _productRepository.GetProductByIdAsync(command.Uuid);
            if (product == null)
                return default;

            product.Name = command.Name;
            product.Description = command.Description;
            product.ImageUrl = command.ImageUrl;
            product.Category = command.Category;


            return await _productRepository.UpdateProductAsync(product);
        }
        private void ValidateCommand(UpdateProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Description))
            {
                throw new ArgumentException("The description cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(command.ImageUrl))
            {
                throw new ArgumentException("The picture cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentException("The name cannot be empty.");
            }
            if (command.Price < 0)
            {
                throw new ArgumentException("The price must be greater than zero.");
            }
            if (command.Uuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
            if (command.Category == Guid.Empty)
            {
                throw new ArgumentException("The category cannot be empty.");
            }
        }
    }
}