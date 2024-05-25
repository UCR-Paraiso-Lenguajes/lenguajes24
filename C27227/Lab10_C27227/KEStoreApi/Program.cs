using Core;
using KEStoreApi.Data;

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

builder.Configuration.AddJsonFile("appsettings.json", optional:false, reloadOnChange:true);
string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();
DatabaseConfiguration.Init(connection);
// Add CORS

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DatabaseStore.Store_MySql();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
