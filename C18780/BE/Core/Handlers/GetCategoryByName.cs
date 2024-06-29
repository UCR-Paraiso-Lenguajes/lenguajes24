using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;
namespace StoreApi.Handler
{
    public sealed class GetCategoryByNameHandler : IRequestHandler<GetCategoryByNameQuery, Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByNameHandler(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentException("Illegal action, category repository is invalid.");
            }
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> Handle(GetCategoryByNameQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);

            return await _categoryRepository.GetCategoryByNameAsync(query.Name);
        }

        private void ValidateQuery(GetCategoryByNameQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.Name))
            {
                throw new ArgumentException("The name cannot be empty.");
            }
        }
    }
}
