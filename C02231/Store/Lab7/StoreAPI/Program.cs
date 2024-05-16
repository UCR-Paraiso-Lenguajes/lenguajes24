using Core;
using StoreAPI.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
var app = builder.Build();

var value = Environment.GetEnvironmentVariable("DB");

if (value == null)
{
    builder.Configuration.AddJsonFile("C:/Users/Lani0/OneDrive/Documents/UCR/Lenguajes/lenguajes24/C02231/Store/Lab7/StoreAPI/appsettings.json", optional: true, reloadOnChange: true);
    connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();
}
else
{
    connection = value;
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Storage.Init(connection);
StoreDB.CreateMysql();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();
