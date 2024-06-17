using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace UT
{
    [TestFixture]
    public class AuthLogicTests
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IHostEnvironment> _mockHostEnvironment;
        private AuthLogic _authLogic;
        [SetUp]
        public void Setup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json");

            IConfiguration configuration = configurationBuilder.Build();
            _mockHostEnvironment = new Mock<IHostEnvironment>();
            _mockHostEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);

            _authLogic = new AuthLogic(_mockHostEnvironment.Object, configuration);
        }

        [Test]
        public void ValidateUser_ValidCredentials_ReturnsTrue()
        {
            var user = new UserAuth { Name = "admin", Password = "admin123" };

            var result = _authLogic.ValidateUser(user, out var claims);

            Assert.IsTrue(result);
            Assert.IsNotNull(claims);
            Assert.AreEqual(2, claims.Count);
            Assert.AreEqual("admin", claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value);
            Assert.AreEqual("Admin", claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
        }

        [Test]
        public void ValidateUser_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var user = new UserAuth { Name = "admin", Password = "wrongpassword" };

            // Act
            var result = _authLogic.ValidateUser(user, out var claims);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(claims);
        }

        [Test]
        public void ValidateUser_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var user = new UserAuth { Name = "nonexistentuser", Password = "password" };

            // Act
            var result = _authLogic.ValidateUser(user, out var claims);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(claims);
        }

        [Test]
        public void CreateToken_ValidClaims_ReturnsToken()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            // Act
            var token = _authLogic.CreateToken(claims);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            Assert.IsNotNull(securityToken);
            Assert.AreEqual("https://localhost:5072", securityToken.Issuer);
            Assert.AreEqual("https://localhost:5072", securityToken.Audiences.First());
        }

        [Test]
        public void CreateToken_NullClaims_ThrowsException()
        {
            // Arrange
            List<Claim> claims = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _authLogic.CreateToken(claims));
        }
    }
}
