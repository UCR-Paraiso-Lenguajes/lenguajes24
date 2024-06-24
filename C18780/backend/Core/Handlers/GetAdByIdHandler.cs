using MediatR;
using StoreApi.Queries;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class GetAdByIdHandler : IRequestHandler<GetAdByIdQuery, Ad>
    {
        private readonly IAdRepository _adRepository;
        public GetAdByIdHandler(IAdRepository adRepository)
        {
            if (adRepository == null)
            {
                throw new ArgumentException("Illegal action, adRepository is invalid.");
            }
            _adRepository = adRepository;
        }
        public async Task<Ad> Handle(GetAdByIdQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);
            return await _adRepository.GetAdByIdAsync(query.Uuid);
        }

        private void ValidateQuery(GetAdByIdQuery query)
        {
            if (query.Uuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
        }
    }
}