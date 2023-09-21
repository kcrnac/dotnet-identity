using IdentityService.Abstraction.Configuration;
using IdentityService.Abstraction.Services;
using IdentityService.Entities;
using IdentityService.Infrastructure;
using IdentityService.Infrastructure.Configuration;
using IdentityService.Infrastructure.Configuration.Options;
using IdentityService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService.Extensions;

public static class ServiceExtensions
{
    private const string ConnectionStringName = "Identity";

    public static IServiceCollection AppIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddDbContext<AppIdentityDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConnectionStringName));
        });

        services.AddIdentityCore<User>(options =>
        {
            // identity options
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddSignInManager<SignInManager<User>>();

        var jwtOptions = new JwtOptions();
        configuration.GetSection(jwtOptions.GetSectionName()).Bind(jwtOptions);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static void AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationOptions = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(p => p.GetTypes())
            .Where(p => typeof(IConfigurationOption).IsAssignableFrom(p) && !p.IsInterface);

        foreach (var type in configurationOptions)
        {
            try
            {
                var configureOptionType = typeof(ConfigureOptionsBase<>).MakeGenericType(new[] { type });
                var configurationOptionObj = Activator.CreateInstance(type) as IConfigurationOption;
                var configureOptionObj = Activator.CreateInstance(configureOptionType, configuration, configurationOptionObj!.GetSectionName());

                services.ConfigureOptions(configureOptionObj!);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public static async Task RunDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppIdentityDbContext>();
        var userManager = services.GetService<UserManager<User>>();
        var logger = services.GetService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            await AppIdentityDbContext.SeedUserAsync(userManager);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during migration");
        }
    }
}