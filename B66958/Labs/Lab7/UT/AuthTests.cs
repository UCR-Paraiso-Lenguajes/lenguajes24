using ApiLab7;
using ApiLab7.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UT;

public class AuthTests
{
    private Mock<IOptions<CredentialOptions>> credentialOptionsMock;
    private Mock<IHostEnvironment> hostEnvironmentMock;

    [OneTimeSetUp]
    public void SetUp()
    {
        credentialOptionsMock = new Mock<IOptions<CredentialOptions>>();
        hostEnvironmentMock = new Mock<IHostEnvironment>();
        hostEnvironmentMock.Setup(h => h.EnvironmentName).Returns("Development");
        credentialOptionsMock.Setup(x => x.Value).Returns(new CredentialOptions
        {
            Development = new List<UserAuth>
            {
                new UserAuth { Name = "Diego", Password = "secret", Role = "Admin" },
                new UserAuth { Name = "Eisenheim", Password = "secret", Role = "Operator" }
            }
        });
    }

    [Test]
    public async Task SucessfullAuthenticationReturnsOK()
    {
        var user = new UserAuth{Name="Diego", Password="secret"};

        AuthController authController = new AuthController(hostEnvironmentMock.Object, credentialOptionsMock.Object);
        var result = await authController.LoginAsync(user);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task ForbiddenAuthenticationReturnsUnathorized()
    {
        var user = new UserAuth{Name="Diego", Password="incorrecta"};

        AuthController authController = new AuthController(hostEnvironmentMock.Object, credentialOptionsMock.Object);
        var result = await authController.LoginAsync(user);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }
}