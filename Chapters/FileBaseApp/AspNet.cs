#:sdk Microsoft.NET.Sdk.Web

var app = WebApplication.CreateBuilder(args).Build();
app.MapGet("/", () => "Hello World!");
app.Run();
