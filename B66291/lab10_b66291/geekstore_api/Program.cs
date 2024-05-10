using core.DataBase;

var builder = WebApplication.CreateBuilder(args);

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDatabase").Value.ToString();
string connectionStringMyDb = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDB").Value.ToString();
Storage.Init(connection, connectionStringMyDb) ;

if (app.Environment.IsDevelopment())
{
    StoreDb.CrearDatos();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();