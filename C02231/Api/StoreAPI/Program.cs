using Core;
using StoreAPI.Database;
using StoreAPI.Hubs;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddSignalR();
builder.Services.AddSingleton<CampaignDB>();

var security = Environment.GetEnvironmentVariable("Security");

if (string.IsNullOrEmpty(security) || security.ToLower() == "false")
{
    // Swagger configuration when security is false or not set
    builder.Services.AddSwaggerGen(setup =>
    {
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
}
else
{
    // JWT Authentication configuration when security is set and not false
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:5207",
                ValidAudience = "http://localhost:5207",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Authentication failed:");
                    Console.WriteLine(context.Exception);
                    return Task.CompletedTask;
                }
            };
        });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
string connection;
var dbConnection = Environment.GetEnvironmentVariable("DB");

if (string.IsNullOrEmpty(dbConnection))
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    connection = builder.Configuration.GetConnectionString("MyDatabase");
}
else
{
    connection = dbConnection;
}

Storage.Init(connection);

if (app.Environment.IsDevelopment())
{
    StoreDB.CreateMysql();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

if (!string.IsNullOrEmpty(security) && security.ToLower() != "false")
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapHub<CampaignHub>("/campaignHub");
app.MapControllers();
app.Run();