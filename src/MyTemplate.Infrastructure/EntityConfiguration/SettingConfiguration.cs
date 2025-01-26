using Common.Entities.Setting;

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