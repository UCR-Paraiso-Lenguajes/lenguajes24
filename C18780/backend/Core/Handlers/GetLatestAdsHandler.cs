using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class GetLatestAdsHandler : IRequestHandler<GetLatestAdsQuery, IEnumerable<Ad>>
    {
        private readonly IAdRepository _adRepository;

        public GetLatestAdsHandler(IAdRepository adRepository)
        {
            if (adRepository == null)
            {
                throw new ArgumentException("Illegal action, adRepository is invalid.");
            }
            _adRepository = adRepository;
        }

        public async Task<IEnumerable<Ad>> Handle(GetLatestAdsQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);
            return await _adRepository.GetLatestAdsAsync(query.Count);
        }

        private void ValidateQuery(GetLatestAdsQuery query)
        {
            if (query.Count <= 0)
            {
                throw new ArgumentException("The count must be greater than 0.");
            }
        }
    }
}