using core;
using geekstore_api.Controllers.AuthController;
namespace UT;
using Moq;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


public class TestUser
{
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public List<string> UserRoles { get; set; }
}

public class UserAccountTest
{

    private AuthController authController;
    private Mock<IHostEnvironment> mockHostEnvironment;


    [SetUp]
    public void SetUp()
    {

        mockHostEnvironment = new Mock<IHostEnvironment>();
        mockHostEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
        authController = new AuthController(mockHostEnvironment.Object);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var testUsers = configuration.GetSection("TestUsers").Get<List<TestUser>>();

        foreach (var user in testUsers)
        {
            new UserAccount(user.UserName, user.UserPassword, user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList());
        }

    }

    [Test]
    public async Task usuario_valido()
    {
        var userData = new LoginModel("jaziel", "12345");
        var response = await authController.LoginAsync(userData) as OkObjectResult;
        Assert.NotNull(response);
        Assert.AreEqual(200, response.StatusCode);
        Assert.NotNull(response.Value);
        var token = (response.Value as AuthenticatedResponse)?.Token;
        Assert.NotNull(token);
        Assert.IsNotEmpty(token);
    }

    [Test]
    public async Task usuario_invalido_ingresado()
    {
        var userData = new LoginModel("invaliduser", "invalidpassword");
        var response = await authController.LoginAsync(userData) as UnauthorizedResult;
        Assert.NotNull(response);
        Assert.AreEqual(401, response.StatusCode);
    }

    [Test]
    public async Task usuario_no_existente()
    {
        var userData = new LoginModel("nonexistentuser", "invalidpassword");
        var response = await authController.LoginAsync(userData) as UnauthorizedResult;
        Assert.NotNull(response);
        Assert.AreEqual(401, response.StatusCode);
    }
    
}