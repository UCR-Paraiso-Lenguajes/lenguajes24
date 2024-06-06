using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IDailySalesRepository
    {
        public Task<IEnumerable<DailySales>> GetDailySalesListAsync(DateTime dateTime);
    }
}
