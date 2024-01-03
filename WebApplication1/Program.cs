using Telegram.Bot;
using WebApplication1;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var key = builder.Configuration["TelegramKey"];

var client = new TelegramBotClient(key);

services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
services.AddSingleton<RedisService>();
services.AddSingleton<ITelegramBotClient>(client);
services.AddSingleton<BCGameService>();
services.AddHostedService<CoreService>();

var app = builder.Build();

app.MapGet("/",
    () => Results.Ok("Hello, world"));

app.Run();
