using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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

    [TestFixture]
    public class UserSecurityTest
    {
        private UserAuthController userAuthController;
        private Mock<IWebHostEnvironment> mockHostEnvironment;
        private Mock<IConfiguration> mockConfiguration;
        private List<TestUser> testUsers;

        [SetUp]
        public void SetUp()
        {
            mockHostEnvironment = new Mock<IWebHostEnvironment>();
            mockHostEnvironment.Setup(e => e.EnvironmentName).Returns("Development");

            testUsers = new List<TestUser>
            {
                new TestUser { UserName = "jeancarlo", UserPassword = "123456", UserRoles = new List<string> { "User" } }
            };

            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c.GetSection("TestUsers").Get<List<TestUser>>()).Returns(testUsers);

            userAuthController = new UserAuthController(mockHostEnvironment.Object, mockConfiguration.Object);
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
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }

        [Test]
        public async Task InvalidUserName()
        {
            var userData = new UserAuthController.LoginModel { UserName = "invalid", Password = "123456" };
            var response = await userAuthController.LoginAsync(userData);
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }

        [Test]
        public async Task EmptyPassword_ThrowsArgumentException()
        {
            var userData = new UserAuthController.LoginModel { UserName = "jeancarlo", Password = "" };
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await userAuthController.LoginAsync(userData));
            Assert.That(ex.Message, Is.EqualTo("Password cannot be empty"));
        }

        [Test]
        public async Task EmptyUsername_ThrowsArgumentException()
        {
            var userData = new UserAuthController.LoginModel { UserName = "", Password = "123456" };
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await userAuthController.LoginAsync(userData));
            Assert.That(ex.Message, Is.EqualTo("Username cannot be empty"));
        }
    }
}