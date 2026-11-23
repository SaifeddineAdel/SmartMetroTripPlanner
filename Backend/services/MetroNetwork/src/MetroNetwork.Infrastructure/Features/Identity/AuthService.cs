using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MetroNetwork.Application.Common.Exceptions;
using MetroNetwork.Application.Features.Auth.Configuration;
using MetroNetwork.Application.Features.Auth.DTOs;
using MetroNetwork.Application.Features.Auth.Repositories;
using MetroNetwork.Application.Features.Auth.Services;
using MetroNetwork.Domain.Features.Users;

namespace MetroNetwork.Infrastructure.Identity;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly JwtOptions _options;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService,
        IOptions<JwtOptions> options)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
        _options = options.Value;
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null ||
            !user.IsActive ||
            !VerifyPassword(user, request.Password))
        {
            throw new UnauthorizedException(
                "Invalid email or password.");
        }

        var now = DateTime.UtcNow;
        var accessToken = GenerateAccessToken(user);
        var refreshToken = CreateRefreshToken(user.Id, now);

        await _refreshTokenRepository.AddAsync(
            refreshToken.Entity,
            cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(
            cancellationToken);

        return new AuthResponse(
            accessToken.Token,
            accessToken.ExpiresAt,
            refreshToken.RawToken,
            refreshToken.Entity.ExpiresAt);
    }

    public async Task<AuthResponse> RefreshAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var tokenHash = _refreshTokenService.HashToken(
            request.RefreshToken);

        var currentToken = await _refreshTokenRepository.GetByHashAsync(
            tokenHash,
            cancellationToken);

        if (currentToken is null ||
            currentToken.IsRevoked ||
            currentToken.IsExpired(now) ||
            !currentToken.User.IsActive ||
            currentToken.User.Role != User.AdminRole)
        {
            throw new UnauthorizedException(
                "Invalid or expired refresh token.");
        }

        var user = currentToken.User;
        var newRefreshToken = CreateRefreshToken(user.Id, now);

        currentToken.Revoke(now);

        await _refreshTokenRepository.AddAsync(
            newRefreshToken.Entity,
            cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(
            cancellationToken);

        var accessToken = GenerateAccessToken(user);

        return new AuthResponse(
            accessToken.Token,
            accessToken.ExpiresAt,
            newRefreshToken.RawToken,
            newRefreshToken.Entity.ExpiresAt);
    }

    public async Task RevokeAsync(
        RevokeTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var tokenHash = _refreshTokenService.HashToken(
            request.RefreshToken);

        var refreshToken = await _refreshTokenRepository.GetByHashAsync(
            tokenHash,
            cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked)
        {
            throw new UnauthorizedException(
                "Invalid refresh token.");
        }

        refreshToken.Revoke(DateTime.UtcNow);

        await _refreshTokenRepository.SaveChangesAsync(
            cancellationToken);
    }

    private static bool VerifyPassword(User user, string password)
    {
        var result = new PasswordHasher<User>()
            .VerifyHashedPassword(
                user,
                user.PasswordHash,
                password);

        return result != PasswordVerificationResult.Failed;
    }

    private (RefreshToken Entity, string RawToken) CreateRefreshToken(
        int userId,
        DateTime now)
    {
        var rawToken = _refreshTokenService.GenerateToken();

        var entity = new RefreshToken(
            userId,
            _refreshTokenService.HashToken(rawToken),
            now,
            now.AddDays(_options.RefreshTokenExpirationDays));

        return (entity, rawToken);
    }

    private GeneratedAccessToken GenerateAccessToken(User user)
    {
        return _jwtTokenService.GenerateToken(
            user.Id,
            user.Email,
            user.Role);
    }
}