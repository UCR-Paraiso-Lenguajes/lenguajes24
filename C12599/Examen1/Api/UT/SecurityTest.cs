using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Hosting;
using storeapi.Controllers;
using storeapi.Models; // Agregar esta línea
using Microsoft.AspNetCore.Mvc;
using so.Models;
using System.Collections.Generic;
using System.Security.Claims;
namespace UT
{
    public class SecurityTest
    {
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            // Llamar a SeedUsers para asegurarse de que haya datos de usuario disponibles para las pruebas
            UserAccountSeeder.SeedUsers();

            var hostingEnvironmentMock = new Mock<IHostEnvironment>();
            _controller = new AuthController(hostingEnvironmentMock.Object);
        }

        [Test]
        public void Login_ValidUser_ReturnsOkObjectResult()
        {
            // Arrange
            var user = new UserAccount(
                userName: "mariano", 
                userPassword: "123456", 
                userRoles: new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "mariano"),
                    new Claim(ClaimTypes.Role, "Admin")
                });

            // Act
            var result = _controller.Login(user) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var response = result.Value as AuthenticatedResponse;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Token);
        }

        
}
     }