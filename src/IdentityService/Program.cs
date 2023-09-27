using IdentityService.Extensions;
using IdentityService.Middleware;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddConfigureOptions(builder.Configuration);
builder.Services.AddControllers();

if (isDevelopment)
{
    builder.Services.AddSwagger();
}

builder.Services.ConfigureApiBehaviorOptions();

builder.Services.AppIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunDatabaseMigrations();

app.Run();
