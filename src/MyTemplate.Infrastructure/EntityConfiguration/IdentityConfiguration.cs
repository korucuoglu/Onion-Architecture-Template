using Common.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Infrastructure.EntityConfiguration;

public static class IdentityConfiguration
{
    public static void Configure(ModelBuilder builder)
    {
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
    }
}