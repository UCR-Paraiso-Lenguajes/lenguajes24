using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoApi;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IHostEnvironment hostEnvironment;

    public AuthController(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        this.configuration = configuration;
        this.hostEnvironment = hostEnvironment;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
    {
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Password);

        if (user is null)
        {
            return BadRequest("Invalid client request");
        }


        if (hostEnvironment.IsDevelopment())
        {

            if (user.UserName == "jean@gmail.com" && user.Password == "123456")
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "jean@gmail.com"),
                            new Claim(ClaimTypes.Role, "Operator"),
                            new Claim(ClaimTypes.Role, "Admin"),
                            new Claim(ClaimTypes.Role, "Customer")
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
                Console.WriteLine(tokenString);
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