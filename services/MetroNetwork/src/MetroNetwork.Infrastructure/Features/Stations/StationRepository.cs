using Microsoft.EntityFrameworkCore;
using MetroNetwork.Application.Features.Stations;
using MetroNetwork.Domain.Features.Stations;
using MetroNetwork.Infrastructure.Persistence;
using MetroNetwork.Application.Common.DTOs;
using MetroNetwork.Application.Features.Stations.DTOs;

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

    public async Task<PagedResponse<StationResponse>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Stations
            .AsNoTracking()
            .OrderBy(x => x.Name);

        var totalCount = await _context.Stations.CountAsync(cancellationToken);

        var stations = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new StationResponse(
                x.Id,
                x.Name,
                x.Description))
            .ToListAsync(cancellationToken);

        return new PagedResponse<StationResponse>(
            stations,
            pageNumber,
            pageSize,
            totalCount);
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

    public async Task<Station?> GetByIdWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Stations
            .AsNoTracking()
            .Include(x => x.LineStations)
                .ThenInclude(x => x.MetroLine)
                    .ThenInclude(x => x.LineStations)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
}
