using Common.Context;
using Common.Context.Configuration;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Infrastructure.Context;

public sealed class ApplicationDbContext : BaseIdentityDbContext
{
    public DbSet<Product> Product { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        Seed.AddIdentityData(builder);
        Seed.AddSettingData(builder);
    }
}