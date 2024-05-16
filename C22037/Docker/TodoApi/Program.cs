using Core;
using TodoApi.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add CORS
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string connection = "";

var value = Environment.GetEnvironmentVariable("DB");

if (value == null)
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    //builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
    connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();
}
else
{
    connection = value;
    Console.Write(connection);
}

Storage.Init(connection);

StoreDB.CreateMysql();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();