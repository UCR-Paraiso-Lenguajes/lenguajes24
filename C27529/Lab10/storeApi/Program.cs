using Core;
using storeApi.db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add SignalR
builder.Services.AddSignalR();

// Configuring CORS properly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", // Create a named policy
        builder => builder.WithOrigins("http://localhost:3000", "http://localhost:8080", "http://localhost:5164") 
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()); // Allow credentials
});

// Swagger and JWT Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
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

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5164", // Ensure these match the values in your configuration
            ValidAudience = "http://localhost:5164",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });

var app = builder.Build();

// Database configuration
var connection = Environment.GetEnvironmentVariable("DB") ?? builder.Configuration["ConnectionStrings:MyDatabase"];
app.Logger.LogInformation("Connection String: {connection}", connection);
ConnectionDB.Init(connection);
await StoreDB.CreateMysql();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// Apply CORS policy
app.UseCors("AllowSpecificOrigin"); // Use the named policy

app.UseAuthentication();  // Make sure authentication comes before authorization
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<CampaignController>("/Campannas");
    endpoints.MapControllers();
});

app.Run();
