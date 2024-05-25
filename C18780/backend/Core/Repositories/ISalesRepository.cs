using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface ISalesRepository
    {
        public Task<Sales> GetSalesByIdAsync(Guid uuid);
        public Task<Sales> AddSalesAsync(Sales sales);
        public Task<int> UpdateSalesAsync(Sales sales);
        public Task<Sales> GetSalesByPurchaseNumberAsync(string purchaseNumber);
    }
}
