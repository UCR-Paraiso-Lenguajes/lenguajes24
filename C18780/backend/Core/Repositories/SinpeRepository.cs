using Microsoft.Extensions.Configuration;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class SinpeRepository : ISinpeRepository
    {
        private readonly IConfiguration _configuration;
        public SinpeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Sinpe> AddSinpeAsync(Sinpe sinpe)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var result = dbContext.Sinpe.Add(sinpe);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }
    }
}