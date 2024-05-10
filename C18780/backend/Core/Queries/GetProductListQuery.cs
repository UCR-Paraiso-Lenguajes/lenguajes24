using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetProductListQuery :  IRequest<List<Product>>
    {
    }
}