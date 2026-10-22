
using Microsoft.EntityFrameworkCore;
using MetroNetwork.Application.Features.LineStations;
using MetroNetwork.Domain.Features.LineStations;
using MetroNetwork.Infrastructure.Persistence;

namespace MetroNetwork.Infrastructure.Features.LineStations;

public sealed class LineStationRepository : ILineStationRepository
{
    private readonly MetroNetworkDbContext _context;

    public LineStationRepository(MetroNetworkDbContext context)
    {
        _context = context;
    }

    public Task<bool> StationExistsAsync(
        int stationId,
        CancellationToken cancellationToken = default)
    {
        return _context.Stations.AnyAsync(
            x => x.Id == stationId,
            cancellationToken);
    }

    public Task<bool> MetroLineExistsAsync(
        int metroLineId,
        CancellationToken cancellationToken = default)
    {
        return _context.MetroLines.AnyAsync(
            x => x.Id == metroLineId,
            cancellationToken);
    }

    public Task<LineStation?> GetAsync(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken = default)
    {
        return _context.LineStations.FirstOrDefaultAsync(
            x => x.MetroLineId == metroLineId
                 && x.StationId == stationId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<LineStation>> GetByMetroLineIdAsync(
        int metroLineId,
        CancellationToken cancellationToken = default)
    {
        return await _context.LineStations
            .AsNoTracking()
            .Where(x => x.MetroLineId == metroLineId)
            .OrderBy(x => x.Sequence)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken = default)
    {
        return _context.LineStations.AnyAsync(
            x => x.MetroLineId == metroLineId
                 && x.StationId == stationId,
            cancellationToken);
    }

    public Task<bool> SequenceExistsAsync(
        int metroLineId,
        int sequence,
        CancellationToken cancellationToken = default)
    {
        return _context.LineStations.AnyAsync(
            x => x.MetroLineId == metroLineId
                 && x.Sequence == sequence,
            cancellationToken);
    }

    public async Task AddAsync(
        LineStation lineStation,
        CancellationToken cancellationToken = default)
    {
        await _context.LineStations.AddAsync(
            lineStation,
            cancellationToken);
    }

    public void Remove(LineStation lineStation)
    {
        _context.LineStations.Remove(lineStation);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
