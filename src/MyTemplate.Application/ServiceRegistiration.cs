using System.Reflection;
using Common;
using Common.Interfaces.Services;
using Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
           //opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
       });

        services.AddScoped<ICacheService, RedisService>();
    }
}