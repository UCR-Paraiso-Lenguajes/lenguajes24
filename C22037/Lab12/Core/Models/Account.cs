using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TodoApi
{
    public sealed class Account
    {
        public string User { get; private set; }
        public string Password { get; private set; }
        public IEnumerable<Claim> Claims { get; private set; }
        public static readonly List<Account> Users = new List<Account>();
        public static IEnumerable<Account> UsersData => Users.AsReadOnly();

        public Account(string user, string password, List<Claim> claims)
        {
            if (user == null) throw new ArgumentNullException("User cannot be null.");
            if (password == null) throw new ArgumentNullException("Password cannot be null.");
            if (claims == null) throw new ArgumentNullException("Claims cannot be null.");

            User = user;
            Password = password;
            Claims = claims;
            Users.Add(this);
        }

        public static void CreateMockUsers()
        {
            new Account("aaron@gmail.com", "123456", new List<Claim> { new Claim(ClaimTypes.Name, "aaron@gmail.com"), new Claim(ClaimTypes.Role, "Admin") });
            new Account("aaron.chacon@gmal.com", "111111", new List<Claim> { new Claim(ClaimTypes.Name, "aaron.chacon@gmal.com"), new Claim(ClaimTypes.Role, "User") });
            new Account("alonso.chacon@gmail.com", "333333", new List<Claim> { new Claim(ClaimTypes.Name, "alonso.chacon@gmail.com"), new Claim(ClaimTypes.Role, "Admin") });
            new Account("chacon.aaron@gmail.com", "444444", new List<Claim> { new Claim(ClaimTypes.Name, "chacon.aaron@gmail.com"), new Claim(ClaimTypes.Role, "Admin") });
        }

    }
}