using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using Core;

//JWT Authentication
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MyStoreAPI.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase{

        private readonly IHostEnvironment hostEnvironment;
        //Para poder leer desde appsettings.Development.json
        private readonly IConfiguration configurationApp;

        //lista con los usuarios y sus password (se llena dependiendo del constructor)
        private readonly IEnumerable<LoginModel> listOfAllUsers;

        public authController(IHostEnvironment hostEnvironment, IConfiguration configurationApp){            
            this.hostEnvironment = hostEnvironment;
            //Obtenemos las variables de Ambiente para JWT
            this.configurationApp = configurationApp;
        }

        [HttpPost]
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
                this.mockDataUsers();//para llenar la lista estatica de Usuarios en caso de no usar los de appsettings.Development.json

                //revisamos la lista de usuarios estatica y btenemos sus roles
                //bool isUserValid = User.allUsersData.FirstOrDefault(u => u.userName == loginUser.userName && u.userPassword == loginUser.userPassword);
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

                    var jwtGlobal = Environment.GetEnvironmentVariable("JWT_GLOBAL");
                    if(string.IsNullOrEmpty(jwtGlobal)){
                        jwtGlobal = "https://localhost:7161";
                    }

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: jwtGlobal,
                        audience: jwtGlobal,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
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

                new UserAccount("varo", "123456", new List<Claim> {
                    new Claim(ClaimTypes.Name, "varo"),
                    new Claim(ClaimTypes.Role, "Admin")
                });

                new UserAccount("user2", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "user2"),
                    new Claim(ClaimTypes.Role, "Operator")
                });

                new UserAccount("user3", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "user3"),
                    new Claim(ClaimTypes.Role, "Customer")
                });

                new UserAccount("user4", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "user4"),
                    new Claim(ClaimTypes.Role, "Customer")
                });

                new UserAccount("user5", "123", new List<Claim> {
                    new Claim(ClaimTypes.Name, "user5"),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "Customer")
                });
            }            
        }
    }

    

    public class AuthenticatedResponse{
        public string? Token { get; set; }
    }

}
