using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

public class AuthLogic
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;

    private readonly Dictionary<string, (string Password, string Role)> _developmentCredentials;
    public AuthLogic(IHostEnvironment hostEnvironment, IConfiguration configuration)
    {
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;

        _developmentCredentials = new Dictionary<string, (string Password, string Role)>
    {
        { "admin", (_configuration["TestCredentials:admin:Password"], _configuration["TestCredentials:admin:Role"]) },
        { "user", (_configuration["TestCredentials:user:Password"], _configuration["TestCredentials:user:Role"]) }
    };
    }
    public bool ValidateUser(UserAuth user, out List<Claim> claims)
    {
        claims = null;

        Console.WriteLine("Starting validation for user: " + user.Name);

        if (_hostEnvironment.EnvironmentName == Environments.Development && _developmentCredentials != null)
        {
            Console.WriteLine("Environment is Development and _developmentCredentials is not null.");

            if (_developmentCredentials.TryGetValue(user.Name, out var credential))
            {
                Console.WriteLine("Credentials found for user: " + user.Name);

                if (credential.Password == user.Password)
                {
                    Console.WriteLine("Password matches for user: " + user.Name);

                    claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, credential.Role)
                };

                    Console.WriteLine("Claims created for user: " + user.Name);
                    return true;
                }
                else
                {
                    Console.WriteLine("Password does not match for user: " + user.Name);
                }
            }
            else
            {
                Console.WriteLine("Credentials not found for user: " + user.Name);
            }
        }
        else
        {
            Console.WriteLine("Environment is not Development or _developmentCredentials is null.");
        }

        Console.WriteLine("Validation failed for user: " + user.Name);
        return false;
    }


    public string CreateToken(List<Claim> claims)
    {
        if (claims == null)
            throw new ArgumentNullException(nameof(claims));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}