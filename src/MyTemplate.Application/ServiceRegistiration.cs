using System.Reflection;
using System.Text;
using AspNetCoreRateLimit;
using Common;
using Common.Extensions;
using Common.Middlewares;
using Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MyTemplate.Application;

public static class ServiceRegistiration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonServices(Assembly.GetExecutingAssembly(), configuration);

        services.AddControllers()
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter =
                true; // ASP.NET Core'un model doğrulama hatalarını otomatik olarak kontrol edip işlemesini engeller
        });

        ConfigureCors(services, configuration);
        ConfigureAuthentication(services, configuration);
        ConfigureRateLimiting(services, configuration);

        
        services.Configure<MailSetting>(configuration.GetSection("MailSetting"));
        services.AddHealthChecks();
        services.AddHttpClient();
        services.AddHttpContextAccessor();
    }

    private static void ConfigureCors(IServiceCollection services, IConfiguration configuration)
    {
        var clientAppUrl = configuration.GetConfigValue<string?>("ClientApp:Url", false);
        
        if (string.IsNullOrWhiteSpace(clientAppUrl))
        {
            return;
        }
        
        services.AddCors(opt =>
        {
            opt.AddPolicy(name: "CorsPolicy", builder =>
            {
                builder.WithOrigins(clientAppUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
    }

    private static void ConfigureRateLimiting(IServiceCollection services, IConfiguration configuration)
    {
        // Rate Limiting için Memory Cache ekleme
        services.AddMemoryCache();

        // Rate Limiting konfigürasyonu
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }
   
}