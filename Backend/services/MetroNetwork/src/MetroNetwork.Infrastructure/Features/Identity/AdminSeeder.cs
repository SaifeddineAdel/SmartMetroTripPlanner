using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MetroNetwork.Domain.Features.Users;
using MetroNetwork.Infrastructure.Persistence;

namespace MetroNetwork.Infrastructure.Identity;

public static class AdminSeeder
{
    public static async Task SeedAsync(
        IConfiguration configuration,
        MetroNetworkDbContext context,
        CancellationToken cancellationToken = default)
    {
        var email = configuration["BootstrapAdmin:Email"];
        var password = configuration["BootstrapAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Bootstrap admin credentials are not configured.");
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();

        var existingAdmin = await context.Users
            .AnyAsync(
                user => user.Email == normalizedEmail,
                cancellationToken);

        if (existingAdmin)
            return;

        var admin = new User(
            normalizedEmail,
            "TemporaryHash");

        var passwordHash = new PasswordHasher<User>()
            .HashPassword(admin, password);

        admin.UpdatePassword(passwordHash);

        context.Users.Add(admin);

        await context.SaveChangesAsync(cancellationToken);
    }
}