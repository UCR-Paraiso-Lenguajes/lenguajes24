using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public AuthController(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
            Account.CreateMockUsers();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Login user)
        {
            if (user == null) return BadRequest("Invalid client request.");
            if (user.User == null || user.Password == null) return BadRequest("Invalid client request.");

            var existingUser = Account.UsersData.FirstOrDefault(userD =>
                userD.User == user.User && userD.Password == user.Password);

            if (existingUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.User)
                };
                claims.AddRange(existingUser.Claims);

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5087", //antes con https
                    audience: "http://localhost:5087", //antes con https
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(3),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new AuthenticatedResponse { Token = tokenString });
            }

            return Unauthorized();
        }
    }

    public class AuthenticatedResponse
    {
        public string Token { get; set; }
    }
}