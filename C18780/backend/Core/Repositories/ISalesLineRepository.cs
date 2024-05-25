using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface ISalesLineRepository
    {
        public Task<SalesLine> AddSalesLineAsync(SalesLine salesLine);
    }
}
