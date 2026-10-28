namespace MetroNetwork.Application.Features.Auth.Services;

public sealed record GeneratedAccessToken(
    string Token,
    DateTime ExpiresAt);