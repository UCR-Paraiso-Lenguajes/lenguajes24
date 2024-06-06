using NUnit.Framework;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using TodoApi;
using Microsoft.AspNetCore.Hosting;

namespace UT
{
    public class SecurityTest
    {
        private AuthController authController;

        [SetUp]
        public void Setup()
        {
            Account.CreateMockUsers();

            var hostEnvironmentMock = new Mock<IWebHostEnvironment>();
            authController = new AuthController(hostEnvironmentMock.Object);
        }

        [Test]
        public void Login_ValidUser_ReturnsOkObjectResult()
        {
            var user = new Account("aaron@gmail.com", "123456", new List<Claim>{new Claim(ClaimTypes.Name, "aaron@gmail.com"), new Claim(ClaimTypes.Role, "Admin")});
            var validUser = new Login(user.User, user.Password);
            var result = authController.Login(validUser) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var response = result.Value as AuthenticatedResponse;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Token);
        }

        [Test]
        public void Login_InvalidUser_ReturnsUnauthorizedResult()
        {
            var invalidUser = new Login("invalid@gmail.com", "password");

            var result = authController.Login(invalidUser) as UnauthorizedResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }
    }
}