using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetAdListQuery : IRequest<IEnumerable<Ad>>
    {
        
    }
}