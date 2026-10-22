using Microsoft.EntityFrameworkCore;
using MetroNetwork.Application.Features.MetroLines;
using MetroNetwork.Domain.Features.MetroLines;
using MetroNetwork.Infrastructure.Persistence;

namespace MetroNetwork.Infrastructure.Features.MetroLines;

public sealed class MetroLineRepository : IMetroLineRepository
{
    private readonly MetroNetworkDbContext _context;

    public MetroLineRepository(MetroNetworkDbContext context)
    {
        _context = context;
    }

    public async Task<MetroLine?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.MetroLines
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<MetroLine>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.MetroLines
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return await _context.MetroLines.AnyAsync(
            x => x.Name == normalizedName,
            cancellationToken);
    }

    public async Task<bool> HasStationsAsync(
        int metroLineId,
        CancellationToken cancellationToken = default)
    {
        return await _context.LineStations.AnyAsync(
            x => x.MetroLineId == metroLineId,
            cancellationToken);
    }

    public async Task AddAsync(
        MetroLine metroLine,
        CancellationToken cancellationToken = default)
    {
        await _context.MetroLines.AddAsync(
            metroLine,
            cancellationToken);
    }

    public Task DeleteAsync(
        MetroLine metroLine,
        CancellationToken cancellationToken = default)
    {
        _context.MetroLines.Remove(metroLine);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
