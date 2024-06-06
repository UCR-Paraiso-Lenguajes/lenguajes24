using System.Security.Claims;
using NUnit.Framework;
using StoreAPI.models;

namespace StoreAPI.Tests
{
    [TestFixture]
    public class LoginModelTests
    {

        [Test]
        public void Constructor_ValidArguments_ObjectCreatedSuccessfully()
        {
            // Arrange
            string userName = "testUser";
            string userPassword = "testPassword";

            // Act
            LoginModel loginModel = new LoginModel(userName, userPassword);

            // Assert
            Assert.IsNotNull(loginModel);
            Assert.AreEqual(userName, loginModel.userName);
            Assert.AreEqual(userPassword, loginModel.userPassword);
        }

        [Test]
        public void Constructor_EmptyUserName_ThrowsArgumentException()
        {
            // Arrange
            string userName = "";
            string userPassword = "testPassword";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new LoginModel(userName, userPassword));
        }

        [Test]
        public void Constructor_EmptyUserPassword_ThrowsArgumentException()
        {
            // Arrange
            string userName = "testUser";
            string userPassword = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new LoginModel(userName, userPassword));
        }

        [Test]
        public void UserAccountValidArguments_ObjectCreatedSuccessfully()
        {

            string userName = "testUser";
            string userPassword = "testPassword";
            List<Claim> userRoles = new List<Claim> { new Claim(ClaimTypes.Role, "Admin") };

            UserAccount userAccount = new UserAccount(userName, userPassword, userRoles);

            Assert.IsNotNull(userAccount);
            Assert.AreEqual(userName, userAccount.userName);
            Assert.AreEqual(userPassword, userAccount.userPassword);
            CollectionAssert.AreEqual(userRoles, userAccount.userRoles);

        }

        [Test]
        public void EmptyUserRoles_ThrowsArgumentException()
        {
            string userName = "testUser";
            string userPassword = "testPassword";
            List<Claim> userRoles = new List<Claim>();

            Assert.Throws<ArgumentException>(() => new UserAccount(userName, userPassword, userRoles));


        }


        [Test]
        public void Usser_EmptyUserName_ThrowsArgumentException()
        {
            string userName = "";
            string userPassword = "testPassword";
            List<Claim> userRoles = new List<Claim> { new Claim(ClaimTypes.Role, "Admin") };


            Assert.Throws<ArgumentException>(() => new UserAccount(userName, userPassword, userRoles));


        }


        [Test]
        public void UserAccountEmptyUserPassword_ThrowsArgumentException()
        {

            string userName = "testUser";
            string userPassword = "";
            List<Claim> userRoles = new List<Claim> { new Claim(ClaimTypes.Role, "Admin") };


            Assert.Throws<ArgumentException>(() => new UserAccount(userName, userPassword, userRoles));

        }

    }
}
