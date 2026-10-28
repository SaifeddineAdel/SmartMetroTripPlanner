namespace MetroNetwork.Application.Features.Auth.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;

    public int AccessTokenExpirationHours { get; init; } = 1;

    public int RefreshTokenExpirationDays { get; init; } = 7;
}