using System.Reflection;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyTemplate.Application;

public static class ServiceRegistiration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonServices(Assembly.GetExecutingAssembly(), configuration);
    }
}