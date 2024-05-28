using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using StoreApi;

namespace StoreApiTests
{
    [TestFixture]
    public class AuthTests
    {
        private Mock<IConfiguration> _configurationMock;
        private Mock<IHostEnvironment> _hostEnvironmentMock;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _hostEnvironmentMock = new Mock<IHostEnvironment>();

            _configurationMock.Setup(config => config["JwtSettings:SecretKey"]).Returns("superSecretKey@345superSecretKey@345");

            _hostEnvironmentMock.Setup(env => env.EnvironmentName).Returns("Development");

            _authController = new AuthController(_configurationMock.Object, _hostEnvironmentMock.Object);
        }

        [Test]
        public async Task LoginAsync_ReturnsOkResult()
        {
            var loginModel = new LoginModel { UserName = "jean@gmail.com", Password = "123456" };
            var result = await _authController.LoginAsync(loginModel);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task LoginAsync_ReturnsUnauthorizedResult()
        {
            var loginModel = new LoginModel { UserName = "user", Password = "password" };
            var result = await _authController.LoginAsync(loginModel);
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }
    }
}
