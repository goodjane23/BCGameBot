//WebApplication.CreateBuilder инициализирует новый экземпл€р класса WebApplicationBuilder с предварительно настроенными значени€ми по умолчанию.
using Microsoft.Extensions.Configuration;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
services.AddScoped<BCGameService>();
services.AddHostedService<CoreService>();

services.ConfigureOptions(configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
