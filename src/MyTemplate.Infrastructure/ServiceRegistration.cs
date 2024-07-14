using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Infrastructure.EF;

namespace MyTemplate.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.EnableSensitiveDataLogging(true);
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), configure =>
            {
                configure.MigrationsAssembly("Infrastructure");
            });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}