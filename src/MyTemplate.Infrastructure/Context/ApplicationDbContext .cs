using Common.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyTemplate.Domain.Entities;
using MyTemplate.Domain.Entities.Identity;
using MyTemplate.Domain.Entities.Setting;

namespace MyTemplate.Infrastructure.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
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

        // ApplicationUser tablosunu AUTH şemasına taşıyoruz
        builder.Entity<ApplicationUser>(b =>
        {
            b.ToTable(name: "AspNetUsers", schema: "AUTH");
        });

        // ApplicationRole tablosunu AUTH şemasına taşıyoruz
        builder.Entity<ApplicationRole>(b =>
        {
            b.ToTable(name: "AspNetRoles", schema: "AUTH");
        });

        // Diğer Identity tablolarını da AUTH şemasına taşıyoruz
        builder.Entity<IdentityUserClaim<int>>(b =>
        {
            b.ToTable(name: "AspNetUserClaims", schema: "AUTH");
        });

        builder.Entity<IdentityUserLogin<int>>(b =>
        {
            b.ToTable(name: "AspNetUserLogins", schema: "AUTH");
        });

        builder.Entity<IdentityUserToken<int>>(b =>
        {
            b.ToTable(name: "AspNetUserTokens", schema: "AUTH");
        });

        builder.Entity<IdentityRoleClaim<int>>(b =>
        {
            b.ToTable(name: "AspNetRoleClaims", schema: "AUTH");
        });

        builder.Entity<IdentityUserRole<int>>(b =>
        {
            b.ToTable(name: "AspNetUserRoles", schema: "AUTH");
        });

        builder.Entity<Setting>(b =>
        {
            b.ToTable(name: "Setting", schema: "APP");
        });
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
                changedEntity.Entity.CreatedDate = DateTimeOffset.Now.ToDateTime(TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
            }

            else if (changedEntity.State == EntityState.Modified)
            {
                changedEntity.Entity.UpdatedDate = DateTimeOffset.Now.ToDateTime(TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}