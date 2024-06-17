using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Core.Models.Store_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store_API.Controllers; 
using Moq; 
using Microsoft.AspNetCore.Hosting;


namespace UT
{
    public class TestUser
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public List<string> UserRoles { get; set; }
    }

    public class UserSecurityTest
    {
        private UserAuthController userAuthController;
        private Mock<IWebHostEnvironment> mockHostEnvironment;
        private IConfiguration configuration;

        [SetUp]
        public void SetUp()
        {
            mockHostEnvironment = new Mock<IWebHostEnvironment>();
            mockHostEnvironment.Setup(e => e.EnvironmentName).Returns("Development");

            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            userAuthController = new UserAuthController(mockHostEnvironment.Object, configuration);

            var testUsers = configuration.GetSection("TestUsers").Get<List<TestUser>>();
            foreach (var user in testUsers)
            {
                new UserAuth(user.UserName, user.UserPassword, user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList());
            }
        }

        [Test]
        public async Task ValidUserAndGetToken()
        {
            var userData = new UserAuthController.LoginModel { UserName = "jeancarlo", Password = "123456" };

            var response = await userAuthController.LoginAsync(userData) as OkObjectResult;

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(response.Value);
            var token = (response.Value as UserAuthController.AuthenticatedResponse)?.Token;
            Assert.NotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public async Task InvalidPassWord()
        {
            var userData = new UserAuthController.LoginModel { UserName = "jeancarlo", Password = "invalid" };

            var response = await userAuthController.LoginAsync(userData);
            Assert.NotNull(response);
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }

        [Test]
        public async Task InvalidUserName()
        {
            var userData = new UserAuthController.LoginModel { UserName = "invalid", Password = "123456" };

            var response = await userAuthController.LoginAsync(userData);
            Assert.NotNull(response);
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }
    }
}