using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface ISinpeRepository
    {
        public Task<Sinpe> AddSinpeAsync(Sinpe sinpe);
    }
}
