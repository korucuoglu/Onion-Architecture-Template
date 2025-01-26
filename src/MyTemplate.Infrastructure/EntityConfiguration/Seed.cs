using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Infrastructure.EntityConfiguration;

public static class Seed
{
    public static void AddData(ModelBuilder builder)
    {
        const int adminRoleId = 1;
        const int adminUserId = 1;

        builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" });

        var hasher = new PasswordHasher<ApplicationUser>();

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = adminUserId,
                Email = "admin@gmail.com",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123123"),
                SecurityStamp = Guid.NewGuid().ToString()
            }
        );

        builder.Entity<ApplicationUserRole>().HasData(
            new ApplicationUserRole()
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            }
        );

        const int userRoleId = 2;
        const int userId = 2;

        builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            { Id = userRoleId, Name = "User", NormalizedName = "USER" });

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = userId,
                Email = "user@gmail.com",
                UserName = "user",
                NormalizedUserName = "USER",
                NormalizedEmail = "USER@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123123"),
                SecurityStamp = Guid.NewGuid().ToString()
            }
        );

        builder.Entity<ApplicationUserRole>().HasData(
            new ApplicationUserRole()
            {
                RoleId = userRoleId,
                UserId = userId
            }
        );
    }
}