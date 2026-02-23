using Karting.Api.Middleware;
using Karting.Application;
using Karting.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

app.Run();

public partial class Program;
