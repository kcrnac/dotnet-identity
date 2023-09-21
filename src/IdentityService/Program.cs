using IdentityService.Extensions;
using IdentityService.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigureOptions(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddSwagger();
builder.Services.ConfigureApiBehaviorOptions();

builder.Services.AppIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunDatabaseMigrations();

app.Run();
