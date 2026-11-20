using CustomerGreetingService.BusinessLogic;
using CustomerGreetingService.BusinessLogic.Interfaces;
using CustomerGreetingService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IGreetingRepository, GreetingRepository>();
builder.Services.AddScoped<IGreetingBusinessLogic, GreetingBusinessLogic>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
