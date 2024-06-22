using Core;
using StoreAPI.Database;
using StoreAPI.Hubs;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSingleton<CampaignDB>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

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

//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-8.0
//https://medium.com/@chandrashekharsingh25/securing-asp-net-core-web-api-with-jwt-authentication-ff9ecd9ba1ed
//cristianguillenmendez@Cristians-MacBook-Pro Lab7 % cd TodoApi 
//cristianguillenmendez@Cristians-MacBook-Pro TodoApi % dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.4
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            ValidIssuer = "http://localhost:5207",
            ValidAudience = "http://localhost:5207",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
    {
        policy.WithOrigins("http://localhost:5207", "http://localhost:8080", "http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
    });
});

// Add SignalR services

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("C:/Users/Lani0/OneDrive/Documents/UCR/Lenguajes/lenguajes24/C02231/Api/StoreAPI/appsettings.json", optional: true, reloadOnChange: true);
    string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();

    var DB_value = Environment.GetEnvironmentVariable("DB");
    if (!String.IsNullOrEmpty(DB_value))
    {
        connection = DB_value;
    }
    Storage.Init(connection);

    app.UseSwagger();
    app.UseSwaggerUI();

    StoreDB.CreateMysql();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapHub<CampaignHub>("/campaignHub");
app.MapControllers();
app.Run();
