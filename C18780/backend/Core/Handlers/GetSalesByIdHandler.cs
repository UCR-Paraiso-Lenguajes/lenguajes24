using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class GetSalesByIdHandler : IRequestHandler<GetSalesByIdQuery, Sales>
    {
        private readonly ISalesRepository _salesRepository;

        public GetSalesByIdHandler(ISalesRepository salesRepository)
        {
            if (salesRepository == null)
            {
                throw new ArgumentException("Illegal action, salesRepository is invalid.");
            }
            _salesRepository = salesRepository;
        }

        public async Task<Sales> Handle(GetSalesByIdQuery query, CancellationToken cancellationToken)
        {
            ValidateQuery(query);
            return await _salesRepository.GetSalesByIdAsync(query.Uuid);
        }

        private void ValidateQuery(GetSalesByIdQuery query)
        {
            if (query.Uuid == Guid.Empty)
            {
                throw new ArgumentException("The uuid cannot be empty.");
            }
        }
    }
}
