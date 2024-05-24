using core; 
using storeapi.Database; 
 
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
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("MyDbConnection").Value.ToString(); 
DataConnection.Init(connection) ; 
// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment()) 
{ 
    StoreDB.CreateMysql(); 
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
     
} 
 
app.UseHttpsRedirection(); 
 
app.UseAuthorization(); 
 
app.MapControllers(); 
app.UseCors(); 
 
app.Run();