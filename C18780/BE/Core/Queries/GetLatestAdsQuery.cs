using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetLatestAdsQuery : IRequest<IEnumerable<Ad>>
    {
        public int Count { get; set; }
    }
}