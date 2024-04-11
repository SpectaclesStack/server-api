using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using spectaclesStackServer.Data;
using spectaclesStackServer.Interface;
using spectaclesStackServer.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var serverName = Environment.GetEnvironmentVariable("SERVER_NAME")?.ToString();
var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME")?.ToString();
var username = Environment.GetEnvironmentVariable("USERNAME")?.ToString();
var password = Environment.GetEnvironmentVariable("PASSWORD")?.ToString();
var connectionString = "Server="+serverName+";Port=5432;Database="+databaseName+";Username="+username+";Password="+password;

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(connectionString ??
        throw new InvalidOperationException("Connection String not found or invalid"));
});

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAnswersRepository, AnswersRepository>();
builder.Services.AddScoped<IQuestionsRepository, QuestionsRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();