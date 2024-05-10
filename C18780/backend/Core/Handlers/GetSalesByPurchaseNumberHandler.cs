using MediatR;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class GetSalesByPurchaseNumberHandler : IRequestHandler<GetSalesByPurchaseNumberQuery, Sales>
    {
        private readonly ISalesRepository _salesRepository;

        public GetSalesByPurchaseNumberHandler(ISalesRepository salesRepository)
        {
            if (salesRepository == null)
            {
                throw new ArgumentException("Illegal action, salesRepository is invalid.");
            }
            _salesRepository = salesRepository;
        }

        public async Task<Sales> Handle(GetSalesByPurchaseNumberQuery query, CancellationToken cancellationToken)
        {
            return await _salesRepository.GetSalesByPurchaseNumberAsync(query.PurchaseNumber);
        }
    }
}
