using Microsoft.AspNetCore.Identity;
using MyTemplate.Domain.Entities.Identity;
using MyTemplate.Domain.Entities.Setting;

namespace MyTemplate.Infrastructure.EntityConfiguration;

public static class SettingConfiguration
{
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<Setting>(b =>
        {
            b.ToTable(name: "Setting", schema: "APP");
        });
    }
}