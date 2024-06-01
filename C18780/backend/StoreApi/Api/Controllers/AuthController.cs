using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StoreApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreApi;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IHostEnvironment hostEnvironment;
    private readonly IEnumerable<Credentials> credentials;
    public AuthController(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        this.configuration = configuration;
        this.hostEnvironment = hostEnvironment;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
    {
        if (user is null)
        {
            return BadRequest("Invalid client request");
        }

        if (hostEnvironment.IsDevelopment())
        {
            var validUser = credentials
                                .ToList()
                                .FirstOrDefault(cred =>
                                    cred.UserName == user.UserName && cred.Password == user.Password
                                );

            if (validUser is not null)
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, validUser.UserName),
                            new Claim(ClaimTypes.Role, validUser.Rol),
                        };
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new AuthenticatedResponse { Token = tokenString });
            }

        }


        return Unauthorized();
    }
}

public class LoginModel
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class AuthenticatedResponse
{
    public string? Token { get; set; }
}