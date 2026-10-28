namespace MetroNetwork.Application.Features.Auth.Services;

public interface IJwtTokenService
{
    GeneratedAccessToken GenerateToken(
        int userId,
        string email,
        string role);
}