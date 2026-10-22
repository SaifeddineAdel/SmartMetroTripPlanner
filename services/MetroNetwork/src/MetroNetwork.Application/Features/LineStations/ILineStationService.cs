using MetroNetwork.Application.Features.LineStations.DTOs;

namespace MetroNetwork.Application.Features.LineStations;

public interface ILineStationService
{
    Task<LineStationResponse> AddStationToLineAsync(
        int metroLineId,
        CreateLineStationRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LineStationResponse>> GetStationsByLineAsync(
        int metroLineId,
        CancellationToken cancellationToken = default);

    Task ChangeSequenceAsync(
        int metroLineId,
        int stationId,
        int newSequence,
        CancellationToken cancellationToken = default);

    Task RemoveStationFromLineAsync(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken = default);
}
