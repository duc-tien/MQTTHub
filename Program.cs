using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MQTT;
using MQTT.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHostedService<MybackgroundService>();
builder.Services.AddSingleton < ManagedMqtt >();
builder.Services.AddSingleton<Data>();
builder.Services.AddSignalR();
builder.Services.AddDbContext<MyData>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDaTa"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .WithOrigins("localhost", "http://localhost:5173", "http://localhost:3000", "https://vtsweb.vercel.app", "http://192.168.123.199:3000", "https://vt-sweb-8p3scssfj-duc-tien.vercel.app", "http://192.168.1.179:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");
app.MapControllers();
app.MapHub<MyHub>("/myhub");

app.Run();