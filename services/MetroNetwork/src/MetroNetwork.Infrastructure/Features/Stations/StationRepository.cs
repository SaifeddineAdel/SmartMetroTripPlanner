using Microsoft.EntityFrameworkCore;
using MetroNetwork.Application.Features.Stations;
using MetroNetwork.Domain.Features.Stations;
using MetroNetwork.Infrastructure.Persistence;

namespace MetroNetwork.Infrastructure.Features.Stations;

public sealed class StationRepository : IStationRepository
{
    private readonly MetroNetworkDbContext _context;

    public StationRepository(MetroNetworkDbContext context)
    {
        _context = context;
    }

    public async Task<Station?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Stations
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Station>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Stations
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Station station,
        CancellationToken cancellationToken = default)
    {
        await _context.Stations.AddAsync(station, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return await _context.Stations
            .AnyAsync(
                x => x.Name.ToLower() == normalizedName.ToLower(),
                cancellationToken);
    }
}
