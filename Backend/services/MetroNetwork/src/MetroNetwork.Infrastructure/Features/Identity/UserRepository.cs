using MetroNetwork.Application.Features.Auth.Repositories;
using MetroNetwork.Domain.Features.Users;
using MetroNetwork.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MetroNetwork.Infrastructure.Identity;

public sealed class UserRepository : IUserRepository
{
    private readonly MetroNetworkDbContext _context;

    public UserRepository(MetroNetworkDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _context.Users.SingleOrDefaultAsync(
            x => x.Email == normalizedEmail,
            cancellationToken);
    }

    public async Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}