using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StoreApi;
using StoreApi.Commands;
using StoreApi.Data;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Queries;
using StoreApi.Repositories;
using System.Configuration;
using System.Reflection;
using System.Text;
using SignalRWebpack.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<ISalesLineRepository, SalesLineRepository>();
builder.Services.AddScoped<ISinpeRepository, SinpeRepository>();
builder.Services.AddScoped<IDailySalesRepository, DailySalesRepository>();
builder.Services.AddScoped<IWeeklySalesRepository, WeeklySalesRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAdRepository, AdRepository>();

// Registra los manejadores de MediatR específicos
builder.Services.AddTransient<IRequestHandler<GetProductListQuery, List<Product>>, GetProductListHandler>();
builder.Services.AddTransient<IRequestHandler<GetProductByIdQuery, Product>, GetProductByIdHandler>();
builder.Services.AddTransient<IRequestHandler<GetProductByCategoryQuery, List<Product>>, GetProductByCategoryHandler>();
builder.Services.AddTransient<IRequestHandler<CreateProductCommand, Product>, CreateProductHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteProductCommand, int>, DeleteProductHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateProductCommand, int>, UpdateProductHandler>();

builder.Services.AddTransient<IRequestHandler<CreateSalesCommand, Sales>, CreateSalesHandler>();
builder.Services.AddTransient<IRequestHandler<GetSalesByIdQuery, Sales>, GetSalesByIdHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateSalesCommand, int>, UpdateSalesHandler>();
builder.Services.AddTransient<IRequestHandler<GetSalesByPurchaseNumberQuery, Sales>, GetSalesByPurchaseNumberHandler>();

builder.Services.AddTransient<IRequestHandler<CreateSalesLineCommand, SalesLine>, CreateSalesLineHandler>();

builder.Services.AddTransient<IRequestHandler<CreateSinpeCommand, Sinpe>, CreateSinpeHandler>();

builder.Services.AddTransient<IRequestHandler<GetDailySalesQuery, IEnumerable<DailySales>>, GetDailySalesByDateHandler>();

builder.Services.AddTransient<IRequestHandler<GetWeeklySalesByDateQuery, IEnumerable<WeeklySales>>, GetWeeklySalesByDateHandler>();

builder.Services.AddTransient<IRequestHandler<GetCategoryByIdQuery, Category>, GetCategoryByIdHandler>();
builder.Services.AddTransient<IRequestHandler<GetCategoryByNameQuery, Category>, GetCategoryByNameHandler>();
builder.Services.AddTransient<IRequestHandler<GetCategoryListQuery, IEnumerable<Category>>, GetCategoryListHandler>();
builder.Services.AddTransient<IRequestHandler<CreateCategoryCommand, Category>, CreateCategoryHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteCategoryCommand, int>, DeleteCategoryHandler>();

builder.Services.AddTransient<IRequestHandler<CreateAdCommand, Ad>, CreateAdHandler>();
builder.Services.AddTransient<IRequestHandler<GetAdByIdQuery, Ad>, GetAdByIdHandler>();
builder.Services.AddTransient<IRequestHandler<GetAdListQuery, IEnumerable<Ad>>, GetAdListHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteAdCommand, int>, DeleteAdHandler>();
builder.Services.AddTransient<IRequestHandler<GetLatestAdsQuery, IEnumerable<Ad>>, GetLatestAdsHandler>();

//Add services to controllers
builder.Services.AddTransient<CategoryController>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }));

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
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

var configuration = builder.Configuration;
var jwtSettings = configuration.GetSection("JwtSettings");
string secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:5001",
                ValidAudience = "https://localhost:5001",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<CampaignHub>("/hub");
app.MapHub<PaymentMethodsHub>("/paymentMethodsHub");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    string connection = builder.Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value.ToString();

    var DB_value = Environment.GetEnvironmentVariable("DB");
    if (!String.IsNullOrEmpty(DB_value))
    {
        connection = DB_value;
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

var value = Environment.GetEnvironmentVariable("DB");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("corsapp");

app.Run();