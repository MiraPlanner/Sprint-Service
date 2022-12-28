using Mira_Common.MassTransit;
using Mira_Common.MongoDB;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;
using Sprint_Service.Services;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Value.Split(";");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddMongo()
    .AddMongoRepository<Sprint>("sprints")
    .AddMongoRepository<Issue>("issues")
    .AddMassTransitWithRabbitMq(); 

builder.Services.AddTransient<ISprintService, SprintService>();
builder.Services.AddTransient<IIssueService, IssueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();