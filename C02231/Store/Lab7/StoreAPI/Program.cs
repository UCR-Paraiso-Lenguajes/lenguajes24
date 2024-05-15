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

var app = builder.Build();
builder.Configuration.AddJsonFile("C:/Users/Lani0/OneDrive/Documents/UCR/Lenguajes/lenguajes24/C02231/Store/Lab7/StoreAPI/appsettings.json", optional: true, reloadOnChange: true);
//builder.Configuration.AddJsonFile("C:/Users/Lani0/OneDrive/Documents/UCR/Lenguajes/lenguajes24/C02231/Store/Lab7/StoreAPI/appsettings.Development.json", optional: true, reloadOnChange: true);
string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();
Storage.Init(connection) ;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    StoreDB.CreateMysql();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
Console.Write("Hello");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();
