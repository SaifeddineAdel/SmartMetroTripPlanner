using MetroNetwork.Application.Features.Auth.Repositories;
using MetroNetwork.Domain.Features.Users;
using MetroNetwork.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MetroNetwork.Infrastructure.Identity;

public sealed class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly MetroNetworkDbContext _context;

    public RefreshTokenRepository(MetroNetworkDbContext context)
    {
        _context = context;
    }

    public Task<RefreshToken?> GetByHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default)
    {
        return _context.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);
    }

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}