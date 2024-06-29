using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IWeeklySalesRepository
    {
        public Task<IEnumerable<WeeklySales>> GetWeeklySalesByDateAsync(DateTime dateTime);
    }
}
