//WebApplication.CreateBuilder инициализирует новый экземпл€р класса WebApplicationBuilder с предварительно настроенными значени€ми по умолчанию.
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
