using geekstore_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
namespace UT;

public class AuthControllerTest
{

    private AuthController _controller;
    private Mock<IConfiguration> _configurationMock;
    private Mock<IHostEnvironment> _hostEnvironmentMock;

    [SetUp]
    public void Setup()
    {
        _configurationMock = new Mock<IConfiguration>();
        _hostEnvironmentMock = new Mock<IHostEnvironment>();
        _controller = new AuthController(_configurationMock.Object, _hostEnvironmentMock.Object);
    }

    [Test]
    public async Task Loguear_Con_Credenciales_Vacias()
    {
        var loginModel = new LoginModel
        {
            UserName = "",
            Password = ""
        };
        var result = await _controller.LoginAsync(loginModel);
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }
}
