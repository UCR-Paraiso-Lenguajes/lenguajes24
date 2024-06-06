using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetCategoryByIdQuery :  IRequest<Category>
    {
        public Guid Uuid { get; set; }
    }
}