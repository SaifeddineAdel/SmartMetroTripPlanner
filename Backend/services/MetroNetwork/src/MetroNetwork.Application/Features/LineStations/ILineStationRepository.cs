using MetroNetwork.Domain.Features.LineStations;

namespace MetroNetwork.Application.Features.LineStations;

public interface ILineStationRepository
{
    Task<bool> StationExistsAsync(
        int stationId,
        CancellationToken cancellationToken = default);

    Task<bool> MetroLineExistsAsync(
        int metroLineId,
        CancellationToken cancellationToken = default);

    Task<LineStation?> GetAsync(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LineStation>> GetByMetroLineIdAsync(
        int metroLineId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken = default);

    Task<bool> SequenceExistsAsync(
        int metroLineId,
        int sequence,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        LineStation lineStation,
        CancellationToken cancellationToken = default);

    void Remove(LineStation lineStation);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
