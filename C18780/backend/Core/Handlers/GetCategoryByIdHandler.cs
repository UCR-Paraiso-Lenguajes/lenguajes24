using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;
namespace StoreApi.Handler
{
    public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdHandler(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentException("Illegal action, categoryRepository is invalid.");
            }
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);

            return await _categoryRepository.GetCategoryByIdAsync(query.Uuid);
        }

        private void ValidateQuery(GetCategoryByIdQuery query)
        {
            if (query.Uuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
        }
    }
}
