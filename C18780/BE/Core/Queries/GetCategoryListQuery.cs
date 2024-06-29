using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetCategoryListQuery :  IRequest<IEnumerable<Category>>
    {
    }
}