using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class GetAdListHandler : IRequestHandler<GetAdListQuery, IEnumerable<Ad>>
    {
        private readonly IAdRepository _adRepository;
        public GetAdListHandler(IAdRepository adRepository)
        {
            if (adRepository == null)
            {
                throw new ArgumentException("Illegal action, adRepository is invalid");
            }
            _adRepository = adRepository;
        }
        public async Task<IEnumerable<Ad>> Handle(GetAdListQuery query, CancellationToken cancellationToken)
        {
            return await _adRepository.GetAdListAsync();
        }
    }
}