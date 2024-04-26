using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public class GetDailySalesByDateHandler : IRequestHandler<GetDailySalesQuery, List<DailySales>>
    {
        private readonly IDailySalesRepository _dailySalesRepository;

        public GetDailySalesByDateHandler(IDailySalesRepository dailySalesRepository)
        {
            _dailySalesRepository = dailySalesRepository;
        }

        public async Task<List<DailySales>> Handle(GetDailySalesQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);

            return await _dailySalesRepository.GetDailySalesListAsync(query.DateTime);
        }
        private void ValidateQuery(GetDailySalesQuery query)
        {
            if (query.DateTime == DateTime.MinValue)
            {
                throw new ArgumentException("The Date cannot be empty.");
            }
        }
    }
}