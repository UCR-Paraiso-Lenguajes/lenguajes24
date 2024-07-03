using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Data;
using Microsoft.Extensions.Configuration;


namespace StoreApiTests
{
    public class AdsTest
    {
        private IConfiguration _configuration;
        private IAdRepository _adRepository;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();
            _adRepository = new AdRepository(_configuration);
        }

        [Test, Order(1)]
        public async Task AddAdAsync_ValidAd_ReturnsAd()
        {
            var ad = new Ad
            {
                Uuid = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18"),
                Date = DateTime.UtcNow,
                Message = "Test Ad"
            };

            var result = await _adRepository.AddAdAsync(ad);

            Assert.NotNull(result);
            Assert.AreEqual(ad.Uuid, result.Uuid);
        }

        [Test, Order(2)]
        public async Task GetAdByIdAsync_NonExistingId_ReturnsNull()
        {
            var nonExistingAdId = Guid.NewGuid();

            var result = await _adRepository.GetAdByIdAsync(nonExistingAdId);

            Assert.Null(result);
        }

        [Test, Order(3)]
        public async Task DeleteAdAsync_ExistingId_ReturnsOne()
        {
            var result = await _adRepository.DeleteAdAsync(Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18"));

            Assert.AreEqual(1, result);
        }

        [Test, Order(4)]
        public async Task GetAdListAsync_ReturnsAdList()
        {
            var result = await _adRepository.GetAdListAsync();

            Assert.NotNull(result);
            Assert.IsInstanceOf<List<Ad>>(result);
        }

        [Test, Order(5)]
        public async Task GetLatestAdsAsync_ValidCount_ReturnsLatestAds()
        {
            var count = 2;
            var result = await _adRepository.GetLatestAdsAsync(count);

            Assert.NotNull(result);
            Assert.IsInstanceOf<List<Ad>>(result);
            Assert.LessOrEqual(result.Count, count);
        }
    }
}
