using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StoreApi.Models;
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
        credentials = this.configuration.GetSection("Credentials:Staff").Get<List<Credentials>>();

    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
    {
        if (user is null)
        {
            return BadRequest("Invalid client request");
        }
        
        var validUser = credentials.Where(c => c.UserName == user.UserName && c.Password == user.Password).FirstOrDefault();

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
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new AuthenticatedResponse { Token = tokenString });
        }

        //}


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