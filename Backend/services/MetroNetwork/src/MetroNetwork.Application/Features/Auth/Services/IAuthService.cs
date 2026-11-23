using MetroNetwork.Application.Features.Auth.DTOs;

namespace MetroNetwork.Application.Features.Auth.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);

    Task<AuthResponse> RefreshAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default);

    Task RevokeAsync(
        RevokeTokenRequest request,
        CancellationToken cancellationToken = default);
}