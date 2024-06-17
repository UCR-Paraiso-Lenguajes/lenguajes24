
using ApiPlu.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:30001")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });
builder.Services.AddSignalR();

var app = builder.Build();
var hubContext = app.Services.GetService(typeof(IHubContext<ChatHub>));
    Task.Run(async () =>{
        await Task.Delay(10000);
        await ((IHubContext)hubContext).Clients.All.SendAsync("ReceiveMessage", "aaaaa", "aaaaa");
        Console.WriteLine($"Message sent: {"aaaaa"}");

    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.MapHub<ChatHub>("/chatHub");


app.Run();



