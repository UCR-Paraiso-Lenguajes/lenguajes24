var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p =>
    p.AddPolicy(
        "corsapp",
        builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }
    )
);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
string connection = builder
    .Configuration.GetSection("ConnectionStrings")
    .GetSection("SqlServerUCR")
    .Value.ToString();
ApiLab7.Db.BuildDb(connection);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    ApiLab7.Db.CreateDB();
    ApiLab7.Db.FillProducts();
    ApiLab7.Db.FillPaymentMethods();
    ApiLab7.SaleData.InsertSales();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("corsapp");

app.Run();
