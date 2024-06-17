namespace UT;
using storeApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Microsoft.Extensions.FileProviders;

internal class TestHostEnvironment : IHostEnvironment
{
    public string EnvironmentName { get; set; }
    public string ApplicationName { get; set; }
    public string ContentRootPath { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }

}
public class AuthControllerTests
{
    private authController _controller;
    private IHostEnvironment _testEnvironment;

    [SetUp]
    public void Setup()
    {
        _testEnvironment = new TestHostEnvironment
        {
            EnvironmentName = "Development"
        };

        _controller = new authController(_testEnvironment);
    }
    [Test]
    public void TestEnvironmentIsDevelopment()
    {
        Assert.AreEqual("Development", _testEnvironment.EnvironmentName, "El entorno no está configurado en 'Development'.");
    }

    [Test]
    public async Task LoginAsync_NullDataUserLog_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.LoginAsync(null);
        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task LoginAsync_NullOrEmptyUserPass_ReturnsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new LoginMod("bulan", ""));
    }
    [Test]
    public async Task LoginAsync_NullOrEmptyUserLog_ReturnsArgumentException()
    {
        // Arrange
        Assert.Throws<ArgumentException>(() => new LoginMod(string.Empty, "123456"));
    }
    [Test]
    public async Task LoginAsync_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginMod = new LoginMod("textoDePrueba", "textoDePrueba");
        // Act
        var result = await _controller.LoginAsync(loginMod);
        // Assert
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }

    [Test]
    public async Task LoginAsync_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var loginMod = new LoginMod("bulan", "123456");
        // Act
        var result = await _controller.LoginAsync(loginMod);
        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        var responseAuthen = (ResponseAuthen)okResult.Value;
        Assert.IsNotNull(responseAuthen.Token);
    }
   

}
