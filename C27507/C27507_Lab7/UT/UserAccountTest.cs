using MyStoreAPI.Controllers;
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using NUnit.Framework;
//para poder simular el Enviroment en Test
using Moq;
using Microsoft.Extensions.Hosting;
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


        [SetUp]
        public void SetUp(){

            mockHostEnvironment = new Mock<IHostEnvironment>();
            authController = new authController(mockHostEnvironment.Object);
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            // var configuration = new ConfigurationBuilder()
            // .SetBasePath(Directory.GetCurrentDirectory())
            // .AddJsonFile("appsettings.Development.json")
            // .Build();

            var testUsers = configuration.GetSection("TestUsers").Get<List<TestUser>>();

            foreach (var user in testUsers){
                new UserAccount(user.UserName, user.UserPassword, user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList());
            }
        }

        [Test]
        public async Task ValidUserAndGetToken(){
            var userData = new UserAccount("varo", "123456", new List<Claim> {
                new Claim(ClaimTypes.Role, "Admin")
            });

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
            
            var userData = new LoginModel { UserName = "varo", Password = "aaaa" };            
            var response = await authController.LoginAsync(userData);            
            Assert.NotNull(response);
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }

        [Test]
        public async Task InvalidUserName(){            
            var userData = new LoginModel { UserName = "aaaaa", Password = "password" };            
            var response = await authController.LoginAsync(userData);
            Assert.NotNull(response);
            Assert.IsInstanceOf<UnauthorizedResult>(response);
        }
    }
}
