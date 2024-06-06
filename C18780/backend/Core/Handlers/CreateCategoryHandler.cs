using MediatR;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Repositories;
namespace StoreApi.Handler
{
    public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentException("Illegal action, categoryRepository is invalid.");
            }
            _categoryRepository = categoryRepository;
        }
        public async Task<Category> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);
            var category = new Category()
            {
                Name = command.Name
            };
            return await _categoryRepository.AddCategoryAsync(category);
        }

        private void ValidateCommand(CreateCategoryCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentException("The name cannot be empty.");
            }
        }
    }
}