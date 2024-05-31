using StoreAPI;
using StoreAPI.models;
using Core;

//JWT Authentication
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace StoreAPI;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase{

        private readonly IHostEnvironment hostEnvironment;

        public AuthController(IHostEnvironment hostEnvironment){
            this.hostEnvironment = hostEnvironment;
            mockDataUsers();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel userDataFromUI){
            //LoginModel es el equivalente a UserAccountAPI en React, para poder recibir solo el nombre y password
            
            // Accediendo a las propiedades de userDataFromUI
            var userName = userDataFromUI.userName;
            var userPassword = userDataFromUI.userPassword;
            

            if (userDataFromUI is null) return BadRequest("Invalid client request");
            if (userName is null || string.IsNullOrEmpty(userName) ) return BadRequest("Invalid client request");            
            if (userPassword is null || string.IsNullOrEmpty(userPassword) ) return BadRequest("Invalid client request");            

            if (hostEnvironment.IsDevelopment()){
                this.mockDataUsers();

                
                var isUserValid = false;
                var claimsRoleFromUser = new List<Claim>();
                foreach (var thisUser in UserAccount.allUsersData){

                    if(userDataFromUI.userName == thisUser.userName && userDataFromUI.userPassword == thisUser.userPassword){

                        claimsRoleFromUser.AddRange(thisUser.userRoles);
                        isUserValid = true;
                        break;
                    }                    
                }
                //si el usuario existe, creamos el token
                if(isUserValid == true){
                                                                            
                    //creamos el claim.Name con el nombre del usuario
                    var claims = new List<Claim>{
                        new Claim(ClaimTypes.Name, userDataFromUI.userName),                                
                    };
                    //pasamos los roles del usuario al claim general
                    claims.AddRange(claimsRoleFromUser);    

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5207",
                        audience: "http://localhost:5207",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return Ok(new AuthenticatedResponse { Token = tokenString });
                }
            }
            return Unauthorized();
        }


        private void mockDataUsers(){

            if (!UserAccount.allUsersData.Any()){

                new UserAccount("lani", "123456", new List<Claim> {
                    new Claim(ClaimTypes.Name, "lani"),
                    new Claim(ClaimTypes.Role, "Admin")
                });

                new UserAccount("paula", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "paula"),
                    new Claim(ClaimTypes.Role, "Operator")
                });

                new UserAccount("user1", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "user1"),
                    new Claim(ClaimTypes.Role, "Customer")
                });

                new UserAccount("user2", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "user2"),
                    new Claim(ClaimTypes.Role, "Customer")
                });

                new UserAccount("user3", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "user3"),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "Customer")
                });
            }            
        }
    }

    

    public class AuthenticatedResponse{
        public string? Token { get; set; }
    }
