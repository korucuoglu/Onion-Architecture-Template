using System.Reflection;
using System.Text;
using Common;
using Common.Interfaces;
using Common.Interfaces.Services;
using Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyTemplate.Infrastructure.Services;

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

        services.AddSingleton<ICacheService, RedisService>();
        services.AddSingleton<IHashService, HashService>();

        services.AddCors(opt =>
        {
            opt.AddPolicy(name: "CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        #region JWT

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
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

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        services.AddScoped<IMessageService, MessageService>();

        #endregion JWT
    }
}