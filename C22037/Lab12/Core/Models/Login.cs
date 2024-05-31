using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TodoApi
{
    public sealed class Login
    {
        public string? User { get; set; }
        public string? Password { get; set; }

       public Login(string user, string password)
       {
            User = user;
            Password = password;
       }
    }
}