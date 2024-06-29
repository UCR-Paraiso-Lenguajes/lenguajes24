using System.Collections;
using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;
namespace StoreApi.Handler
{
    public sealed class GetCategoryListHandler : IRequestHandler<GetCategoryListQuery, IEnumerable<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryListHandler(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentException("Illegal action, category repository is invalid.");
            }
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> Handle(GetCategoryListQuery query, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetCategoryListAsync();
        }
    }
}
