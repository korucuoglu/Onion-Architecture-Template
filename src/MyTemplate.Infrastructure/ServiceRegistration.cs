using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Application.ApplicationManagement.Interfaces;
using MyTemplate.Domain.Entities;
using MyTemplate.Infrastructure.Services;

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

        services.AddSingleton<ISettingService>(provider =>
        {
            using var scope = provider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>().EF;

            var settingRepository = unitOfWork.GetRepository<Setting, int>();

            var settings = settingRepository.GetAll().ToList();

            return new SettingService(settings);
        });
    }
}