using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetProductByCategoryQuery :  IRequest<List<Product>>
    {
        public Guid Category {get; set;}
    }
}