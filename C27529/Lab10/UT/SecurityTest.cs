/*
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using storeApi.Models;
using System.Threading.Tasks;
using System;
using storeApi.Database;
using System.Net.Http;


namespace UT
{
    public class SecurityTest
    {
        private SaleController _controller;
        private Mock<HttpContent> _mockHttpContext;
        private Mock<SaleDB> _mockSaleDB;


        [SetUp]
        public void Setup()
        {
            // Setup mock HttpContext for user authentication
            _mockHttpContext = new Mock<HttpContext>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "AdminUser"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _mockHttpContext.Setup(c => c.User).Returns(claimsPrincipal);


            var controllerContext = new ControllerContext()
            {
                HttpContext = _mockHttpContext.Object
            };


            // Setup mock SaleDB
            _mockSaleDB = new Mock<SaleDB>();
            _mockSaleDB.Setup(db => db.getWeekSalesAsync(It.IsAny<DateTime>())).ReturnsAsync(new Dictionary<string, decimal>());
            _mockSaleDB.Setup(db => db.getDailySales(It.IsAny<DateTime>())).ReturnsAsync(new Dictionary<string, decimal>());
            
            // Initialize the controller with the mocked HttpContext
            _controller = new SaleController(_mockSaleDB.Object)
            {
                ControllerContext = controllerContext
            };
        }


        [Test]
        public async Task GetSale_ValidUserAndDate_ReturnsOkResult()
        {
            // Arrange
            var dateInfo = new SaleController.WeekDailyDate
            {
                WeekDate = DateTime.Now,
                DailyDate = DateTime.Now
            };


            // Act
            var result = await _controller.GetSale(dateInfo);


            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetSale_InvalidDate_ReturnsBadRequest()
        {
            // Arrange
            var dateInfo = new SaleController.WeekDailyDate
            {
                WeekDate = DateTime.MinValue, // Invalid date
                DailyDate = DateTime.MinValue  // Invalid date
            };


            // Act
            var result = await _controller.GetSale(dateInfo);


            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


        [Test]
        public void GetSale_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Change the user to simulate unauthorized access
            var unauthorizedClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "NormalUser"),
                new Claim(ClaimTypes.Role, "User")  // Non-admin role
            };
            var unauthorizedIdentity = new ClaimsIdentity(unauthorizedClaims, "TestAuthType");
            var unauthorizedClaimsPrincipal = new ClaimsPrincipal(unauthorizedIdentity);
            _mockHttpContext.Setup(c => c.User).Returns(unauthorizedClaimsPrincipal);


            // Arrange
            var dateInfo = new SaleController.WeekDailyDate
            {
                WeekDate = DateTime.Now,
                DailyDate = DateTime.Now
            };


            // Act
            var result = _controller.GetSale(dateInfo).Result;


            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }
    }

    internal class SaleController
    {
    }
}
*/




