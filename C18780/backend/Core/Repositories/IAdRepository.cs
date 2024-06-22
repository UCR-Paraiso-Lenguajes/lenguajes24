using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IAdRepository
    {
        public Task<List<Ad>> GetAdListAsync();
        public Task<Ad> GetAdByIdAsync(Guid uuid);
        public Task<Ad> AddAdAsync(Ad ad);
        public Task<int> DeleteAdAsync(Guid uuid);
    }
}
