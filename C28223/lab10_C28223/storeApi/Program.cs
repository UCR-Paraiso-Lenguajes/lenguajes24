using storeApi.DataBase;
using Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();
string connectionStringMyDb = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDB").Value.ToString();
Storage.Init(connection, connectionStringMyDb) ;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    StoreDataBase.CreateMysql();
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