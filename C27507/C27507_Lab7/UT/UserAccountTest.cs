//para usar el "ConfigurationBuilder "
using Microsoft.Extensions.Configuration;

using MyStoreAPI.Controllers;
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using NUnit.Framework;
//para poder simular el Enviroment en Test
using Moq;
using Microsoft.Extensions.Hosting;
//JWT Authentication
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace UT{

    public class TestUser{
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        //Los obtenemos como strings, ya que en el appsettings.Development.json se guardan los roles como al
        public List<string> UserRoles { get; set; }
    }

    public class UserAccountTest{

        private authController authController;
        private Mock<IHostEnvironment> mockHostEnvironment;
        private IConfiguration configuration;



        [SetUp]
        public void SetUp(){

            mockHostEnvironment = new Mock<IHostEnvironment>();
            mockHostEnvironment.Setup(e => e.EnvironmentName).Returns("Development");

            var inMemorySettings = new Dictionary<string, string> {
                {"JWT_GLOBAL", "https://localhost:7161"}
            };
            
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .AddJsonFile("appsettings.Development.json")
                .Build();                        

            authController = new authController(mockHostEnvironment.Object, configuration);            
            var testUsers = configuration.GetSection("TestUsers").Get<List<TestUser>>();

            //Reemplaza a mockdataUsers, ahora usamos los usuarios de appsettings.Development.json
            foreach (var user in testUsers){
                new UserAccount(user.UserName, user.UserPassword, user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList());
            }
            
        }

        [Test]
        public async Task ValidUserAndGetToken(){
            var userData = new LoginModel("varo", "123456");

            var response = await authController.LoginAsync(userData) as OkObjectResult;

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(response.Value);
            var token = (response.Value as AuthenticatedResponse)?.Token;
            Assert.NotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public async Task InvalidPassWord(){
            
            var userData = new LoginModel("varo", "aaa");
            var response = await authController.LoginAsync(userData);
            Assert.NotNull(response);
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }

        [Test]
        public async Task InvalidUserName(){            
            var userData = new LoginModel("aaa", "123456");
            var response = await authController.LoginAsync(userData);
            Assert.NotNull(response);
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }
    }
}
