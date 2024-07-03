using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using StoreApi;

namespace StoreApiTests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private IConfiguration configuration;
        private IHostEnvironment hostEnvironment;
        private AuthController authController;

        [SetUp]
        public void Setup()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "JwtSettings:SecretKey", "SecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKey" },
                { "Credentials:Staff:0:UserName", "testUser" },
                { "Credentials:Staff:0:Password", "testPassword" },
                { "Credentials:Staff:0:Rol", "Admin" }
            });
            configuration = configurationBuilder.Build();

            hostEnvironment = new HostingEnvironment();

            authController = new AuthController(configuration, hostEnvironment);
        }

        [Test]
        public void Login_ValidUser_ReturnsToken()
        {
            // Arrange
            var userModel = new LoginModel { UserName = "testUser", Password = "testPassword" };

            // Act
            var result = authController.LoginAsync(userModel).GetAwaiter().GetResult() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<AuthenticatedResponse>(result.Value);
            var response = (AuthenticatedResponse)result.Value;
            Assert.IsNotNull(response.Token);
        }

        [Test]
        public void Login_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var userModel = new LoginModel { UserName = "invalidUser", Password = "invalidPassword" };

            // Act
            var result = authController.LoginAsync(userModel).GetAwaiter().GetResult() as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }

        [Test]
        public void Login_NullUser_ReturnsBadRequest()
        {
            // Act
            var result = authController.LoginAsync(null).GetAwaiter().GetResult() as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid client request", result.Value);
        }
    }
}
