using MetroNetwork.Domain.Features.Users;

namespace MetroNetwork.Domain.Features.Users;

public sealed class RefreshToken
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public User User { get; private set; } = null!;

    public string TokenHash { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;

    private RefreshToken()
    {
    }

    public RefreshToken(
        int userId,
        string tokenHash,
        DateTime createdAt,
        DateTime expiresAt)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("Token hash is required.");

        if (expiresAt <= createdAt)
            throw new ArgumentException("Expiry must be after creation.");

        UserId = userId;
        TokenHash = tokenHash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }

    public bool IsExpired(DateTime now) =>
        ExpiresAt <= now;

    public void Revoke(
        DateTime now)
    {
        if (IsRevoked)
            throw new InvalidOperationException(
                "Refresh token has already been revoked.");

        RevokedAt = now;
    }
}