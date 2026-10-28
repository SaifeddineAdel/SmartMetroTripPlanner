namespace MetroNetwork.Application.Features.Auth.Services;

public interface IRefreshTokenService
{
    string GenerateToken();

    string HashToken(string token);
}