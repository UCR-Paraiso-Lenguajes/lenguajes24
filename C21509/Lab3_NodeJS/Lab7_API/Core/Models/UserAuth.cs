using System.Security.Claims;

namespace Core.Models
{

    namespace Store_API.Models
    {
        public class UserAuth
        {
            public string UserName { get; }
            public string Password { get; }
            public IEnumerable<Claim> Roles { get; }

            public static readonly List<UserAuth> allUsers = new List<UserAuth>();

            public static IEnumerable<UserAuth> AllUsersData => allUsers.AsReadOnly();

            public UserAuth(string userName, string password, List<Claim> roles)
            {
                if (roles == null || roles.Count == 0)
                    throw new ArgumentException("Must create roles for users");
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentException(" Null users are not allowed");
                if (string.IsNullOrEmpty(password))
                    throw new ArgumentException("Password is required");

                UserName = userName;
                Password = password;
                Roles = new List<Claim>(roles);

                allUsers.Add(this);
            }
        }

        public static class UserAuthCredentials
        {
            public static void UsersCredentials()
            {
                new UserAuth("jeancarlo", "123456", new List<Claim>
            {
                new Claim(ClaimTypes.Name, "jeancarlo"),
                new Claim(ClaimTypes.Role, "Admin")
            });

                new UserAuth("user2", "654321", new List<Claim>
            {
                new Claim(ClaimTypes.Name, "user2"),
                new Claim(ClaimTypes.Role, "Operator"),
            });

                new UserAuth("user3", "246800", new List<Claim>
            {
                new Claim(ClaimTypes.Name, "user3"),
                new Claim(ClaimTypes.Role, "Costumer")
            });
            }
        }
    }
}