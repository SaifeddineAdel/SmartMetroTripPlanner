using MetroNetwork.Domain.Common.Exceptions;

namespace MetroNetwork.Domain.Features.Users;

public sealed class User
{
    public const string AdminRole = "Admin";

    public int Id { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Role { get; private set; } = AdminRole;

    public bool IsActive { get; private set; } = true;

    private User()
    {
    }

    public User(string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainValidationException("Email is required.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainValidationException("Password hash is required.");

        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
    }

    public void UpdatePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainValidationException("Password hash is required.");

        PasswordHash = passwordHash;
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
}