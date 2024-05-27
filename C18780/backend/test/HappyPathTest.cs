using Microsoft.Extensions.Configuration;

namespace StoreApiTests
{
    public class HappyPathTest
    {
        private IConfiguration _configuration;
        
        [SetUp]
        public async Task Setup()
        {
            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();
        }
    }
}