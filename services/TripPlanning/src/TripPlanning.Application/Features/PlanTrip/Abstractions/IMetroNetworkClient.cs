using TripPlanning.Application.Features.PlanTrip.DTOs;

namespace TripPlanning.Application.Features.PlanTrip.Abstractions;

public interface IMetroNetworkClient
{
    Task<IReadOnlyList<StationData>> GetStationsAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<MetroLineData>> GetMetroLinesAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<LineStationData>> GetLineStationsAsync(
        int metroLineId,
        CancellationToken cancellationToken);
}