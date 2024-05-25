using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class GetWeeklySalesByDateHandler : IRequestHandler<GetWeeklySalesByDateQuery, IEnumerable<WeeklySales>>
    {
        private readonly IWeeklySalesRepository _weeklySalesRepository;
        public GetWeeklySalesByDateHandler(IWeeklySalesRepository weeklySalesRepository)
        {
            if (weeklySalesRepository == null)
            {
                throw new ArgumentException("Illegal action, weeklySalesRepository is invalid.");
            }
            _weeklySalesRepository = weeklySalesRepository;
        }
        public async Task<IEnumerable<WeeklySales>> Handle(GetWeeklySalesByDateQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);
            return await _weeklySalesRepository.GetWeeklySalesByDateAsync(query.DateTime);
        }

        private void ValidateQuery(GetWeeklySalesByDateQuery query)
        {
            if (query.DateTime == DateTime.MinValue)
            {
                throw new ArgumentException("The Date cannot be empty.");
            }
        }
    }
}