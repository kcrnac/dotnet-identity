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
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using IdentityService.Models;

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

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        var jwtOptions = new JwtOptions();
        configuration.GetSection(jwtOptions.GetSectionName()).Bind(jwtOptions);

        services.AddAuthentication(options =>
            {
                /* TODO: Document this
                 *  Using services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) doesn't work properly and uses Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[12] AuthenticationScheme: Identity.Application was challenged. instead of Bearer
                 *  Seems that issue started appear after changing services.AddIdentityCore -> services.AddIdentity
                 *  And once custom Roles were introduced into Identity
                 *  Logs
                 *      Request starting HTTP/2 GET https://localhost:5001/api/Home/authenticated
                        info: Microsoft.AspNetCore.Authorization.DefaultAuthorizationService[2]
                              Authorization failed. These requirements were not met:
                              DenyAnonymousAuthorizationRequirement: Requires an authenticated user.
                        info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[12]
                              AuthenticationScheme: Identity.Application was challenged.
                 */
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
        var roleManager = services.GetService<RoleManager<Role>>();
        var logger = services.GetService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            await AppIdentityDbContext.SeedDatabaseAsync(userManager, roleManager);
        }
        catch (Exception ex)
        {
            logger!.LogError(ex, "An error occurred during migration");
        }
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }

    public static void ConfigureApiBehaviorOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(m => m.Value.Errors)
                    .Select(m => m.ErrorMessage)
                    .ToArray();

                var response = new ApiValidationErrorResponse(errors);

                return new BadRequestObjectResult(response);
            };
        });
    }
}