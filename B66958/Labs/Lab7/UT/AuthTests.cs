using ApiLab7;
using ApiLab7.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
        credentialOptionsMock
            .Setup(x => x.Value)
            .Returns(
                new CredentialOptions(
                    new List<UserAuth>
                    {
                        new UserAuth("Diego", "secret", "Admin"),
                        new UserAuth("Eisenheim", "secret", "Operator")
                    }
                )
            );
    }

    [Test]
    public async Task SucessfullAuthenticationReturnsOK()
    {
        var user = new UserAuth("Diego", "secret", null);

        AuthController authController = new AuthController(
            hostEnvironmentMock.Object,
            credentialOptionsMock.Object
        );
        var result = await authController.LoginAsync(user);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task ForbiddenAuthenticationReturnsUnathorized()
    {
        var user = new UserAuth("Diego", "incorrecta", null);

        AuthController authController = new AuthController(
            hostEnvironmentMock.Object,
            credentialOptionsMock.Object
        );
        var result = await authController.LoginAsync(user);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }
}
