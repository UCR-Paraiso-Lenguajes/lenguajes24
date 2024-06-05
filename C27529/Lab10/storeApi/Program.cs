using Core; 
using storeApi.db; 
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; 
using System.Text; 
using Microsoft.OpenApi.Models; 

 
 
var builder = WebApplication.CreateBuilder(args); 
 
// Add services to the container. 
builder.Services.AddControllers(); 
 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle 
builder.Services.AddEndpointsApiExplorer(); 
//builder.Services.AddSwaggerGen(); 
builder.Services.AddSwaggerGen(setup => 
{ 
    // Include 'SecurityScheme' to use JWT Authentication 
    var jwtSecurityScheme = new OpenApiSecurityScheme 
    { 
        BearerFormat = "JWT", 
        Name = "JWT Authentication", 
        In = ParameterLocation.Header, 
        Type = SecuritySchemeType.Http, 
        Scheme = JwtBearerDefaults.AuthenticationScheme, 
        Description = "Put *ONLY* your JWT Bearer token on textbox below!", 
 
        Reference = new OpenApiReference 
        { 
            Id = JwtBearerDefaults.AuthenticationScheme, 
            Type = ReferenceType.SecurityScheme 
        } 
    }; 
 
    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme); 
 
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement 
    { 
        { jwtSecurityScheme, Array.Empty<string>() } 
    }); 
 
}); 
 
// Add CORS 
builder.Services.AddCors(options => 
{ 
    options.AddDefaultPolicy(builder => 
    { 
        builder.AllowAnyOrigin() 
               .AllowAnyMethod() 
               .AllowAnyHeader(); 
    }); 
}); 
string connection = null; 
 
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-8.0 
//https://medium.com/@chandrashekharsingh25/securing-asp-net-core-web-api-with-jwt-authentication-ff9ecd9ba1ed 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) 
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, 
        options => 
    { 
        options.TokenValidationParameters = new TokenValidationParameters 
        { 
            ValidateIssuer = true, 
            ValidateAudience = true, 
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true, 
            ValidIssuer = "https://localhost:7280", 
            ValidAudience = "https://localhost:7280", 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere")) 
        }; 
    }); 
 
 
var app = builder.Build(); 
 
 
 
var value = Environment.GetEnvironmentVariable("DB"); 
 
if (value == null) 
{ 
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); 
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true); 
#pragma warning disable CS8602 // Dereference of a possibly null reference. 
    connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString(); 
#pragma warning restore CS8602 // Dereference of a possibly null reference. 
} 
else 
{ 
    connection = value; 
} 
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed 
StoreDB.CreateMysql(); 
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed 
 
ConnectionDB.Init(connection); 
 
// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment()) 
{ 
 
    app.UseSwagger(); 
app.UseSwaggerUI(); 
} 
 
 
app.UseHttpsRedirection(); 
 
app.UseRouting(); 
 
// Use CORS 
app.UseCors(); 
 
app.UseAuthorization(); 
 
app.MapControllers(); 
 
app.Run(); 
Console.WriteLine("Estoy ejecutando Program");