using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using spectaclesStackServer.Data;
using spectaclesStackServer.Interface;
using spectaclesStackServer.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var configuration = builder.Configuration;

// build connection string
/*var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("{USERNAME}", username)
    .Replace("{PASSWORD}", password)
    .Replace("{SERVERNAME}", serverName)
    .Replace("{DB_NAME}", databaseName);*/

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(connectionString ??
        throw new InvalidOperationException("Connection String not found or invalid"));
});

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAnswersRepository, AnswersRepository>();
builder.Services.AddScoped<IQuestionsRepository, QuestionsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();