using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetDailySalesQuery : IRequest<IEnumerable<DailySales>>
    {
        public DateTime DateTime { get; set; }
    }
}