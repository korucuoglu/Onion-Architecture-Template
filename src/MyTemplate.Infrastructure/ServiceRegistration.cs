using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Domain.Entities;

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
                configure.MigrationsAssembly("MyTemplate.Infrastructure"); // Migrationlar bu projede saklanacak
            });
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}