using MediatR;
using StoreApi.Models;

namespace StoreApi.Queries
{
    public sealed class GetWeeklySalesByDateQuery : IRequest<IEnumerable<WeeklySales>>
    {
        public DateTime DateTime { get; set; }
    }
}