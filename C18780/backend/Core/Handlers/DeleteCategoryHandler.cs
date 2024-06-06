using MediatR;
using StoreApi.Commands;
using StoreApi.Repositories;
namespace StoreApi.Handler
{
    public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, int>
    {
        private readonly ICategoryRepository _categoryRepository;
        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentException("Illegal action, categoryRepository is invalid.");

            }
            _categoryRepository = categoryRepository;
        }
        public async Task<int> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);
            var product = await _categoryRepository.GetCategoryByIdAsync(command.Uuid);
            if (product == null)
                return default;
            return await _categoryRepository.DeleteCategoryAsync(product.Uuid);
        }

        private void ValidateCommand(DeleteCategoryCommand command)
        {
            if (command.Uuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
        }
    }
}