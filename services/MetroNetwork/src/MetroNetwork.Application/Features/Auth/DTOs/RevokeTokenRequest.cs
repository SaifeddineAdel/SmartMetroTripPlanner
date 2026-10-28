namespace MetroNetwork.Application.Features.Auth.DTOs;

public sealed record RevokeTokenRequest(
    string RefreshToken);