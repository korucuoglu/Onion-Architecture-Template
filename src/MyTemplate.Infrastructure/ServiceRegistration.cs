using Common.Entities.Setting;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Application.ApplicationManagement.Repositories.UnitOfWork;
using MyTemplate.Application.ApplicationManagement.Services;
using MyTemplate.Infrastructure.Services;

namespace MyTemplate.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
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

     

        services.AddSingleton<ISettingService>(provider =>
        {
            using var scope = provider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>().EF;

            var settingRepository = unitOfWork.GetRepository<Setting, int>();

            var settings = settingRepository.GetAll().ToList();

            return new SettingService(settings);
        });

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetValue<string>("RabbitMQ:Hostname"), h =>
                {
                    h.Username(configuration.GetValue<string>("RabbitMQ:Username"));
                    h.Password(configuration.GetValue<string>("RabbitMQ:Password"));
                });
            });
        });

        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IUserContextAccessor, UserContextAccessor>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<ICacheService, RedisService>();
        services.AddSingleton<IHashService, HashService>();
    }
}