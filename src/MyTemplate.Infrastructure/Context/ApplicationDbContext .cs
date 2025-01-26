using Common.Entities;
using Common.Entities.Setting;
using Common.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyTemplate.Domain.Entities;
using MyTemplate.Infrastructure.EntityConfiguration;

namespace MyTemplate.Infrastructure.Context;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public DbSet<Product> Product { get; set; }
    public DbSet<Setting> Setting { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = false;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        IdentityConfiguration.Configure(builder);
        SettingConfiguration.Configure(builder);
        Seed.AddData(builder);
    }

    /// <summary>
    /// CreatedDate ve UpdatedDate alanlarını doldurur. 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var changedEntity in ChangeTracker.Entries<IEntityWithDate>())
        {
            if (changedEntity.State == EntityState.Added)
            {
                changedEntity.Entity.CreatedDate =
                    DateTimeOffset.Now.ToDateTime(TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
            }

            else if (changedEntity.State == EntityState.Modified)
            {
                changedEntity.Entity.UpdatedDate =
                    DateTimeOffset.Now.ToDateTime(TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}