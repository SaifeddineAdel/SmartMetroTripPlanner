using TripPlanning.Application.Common.Exceptions;
using TripPlanning.Application.Features.PlanTrip.DTOs;
using TripPlanning.Domain.Features.PlanTrip.Graph;
using TripPlanning.Domain.Features.PlanTrip.Services;

namespace TripPlanning.Application.Features.PlanTrip.Services;

public sealed class PlanTripService
{
    private readonly MetroNetworkStateProvider _stateProvider;
    private readonly MetroPathFinder _pathFinder;
    private readonly TicketFareCalculator _fareCalculator;

    public PlanTripService(
        MetroNetworkStateProvider stateProvider,
        MetroPathFinder pathFinder,
        TicketFareCalculator fareCalculator)
    {
        _stateProvider = stateProvider;
        _pathFinder = pathFinder;
        _fareCalculator = fareCalculator;
    }

    public Task<TripPlanResponse> PlanAsync(
        PlanTripRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var networkState = _stateProvider.GetCurrentState();

        if (!networkState.StationsById.TryGetValue(
            request.StartStationId,
            out var start))
        {
            throw new NotFoundException(
                "The start station was not found.");
        }

        if (!networkState.StationsById.TryGetValue(
            request.DestinationStationId,
            out var destination))
        {
            throw new NotFoundException(
                "The destination station was not found.");
        }

        var path = _pathFinder.FindShortestPath(
            networkState.Graph,
            start.Id,
            destination.Id);

        if (path is null)
        {
            throw new NotFoundException(
                "No route exists between the selected stations.");
        }

        var segments = BuildSegments(
            path,
            networkState.StationsById,
            networkState.MetroLinesById);

        var totalStationsCount = path.Count + 1;

        var totalMinutes = path.Sum(connection =>
            connection.MetroLineId == 1 ? 2.5 : 2.0);

        var fare = _fareCalculator.Calculate(
            totalStationsCount);

        var response = new TripPlanResponse(
            start,
            destination,
            segments,
            totalStationsCount,
            totalMinutes,
            fare);

        return Task.FromResult(response);
    }

    private static IReadOnlyList<TripSegmentResponse> BuildSegments(
        IReadOnlyList<GraphConnection> path,
        IReadOnlyDictionary<int, StationData> stationsById,
        IReadOnlyDictionary<int, MetroLineData> linesById)
    {
        var segments = new List<TripSegmentResponse>();

        if (path.Count == 0)
        {
            return segments;
        }

        var currentConnections = new List<GraphConnection>
        {
            path[0]
        };

        for (var i = 1; i < path.Count; i++)
        {
            if (path[i].MetroLineId != currentConnections[0].MetroLineId)
            {
                segments.Add(CreateSegment(
                    currentConnections,
                    stationsById,
                    linesById,
                    isTransferAfter: true));

                currentConnections = [];
            }

            currentConnections.Add(path[i]);
        }

        segments.Add(CreateSegment(
            currentConnections,
            stationsById,
            linesById,
            isTransferAfter: false));

        return segments;
    }

    private static TripSegmentResponse CreateSegment(
        IReadOnlyList<GraphConnection> connections,
        IReadOnlyDictionary<int, StationData> stationsById,
        IReadOnlyDictionary<int, MetroLineData> linesById,
        bool isTransferAfter)
    {
        var stationIds = new List<int>
        {
            connections[0].FromStationId
        };

        stationIds.AddRange(
            connections.Select(connection => connection.ToStationId));

        var lineId = connections[0].MetroLineId;

        var segmentStations = stationIds
            .Select(id => stationsById[id])
            .ToList();

        var estimatedMinutes = connections.Sum(connection =>
            connection.MetroLineId == 1 ? 2.5 : 2.0);

        return new TripSegmentResponse(
            linesById[lineId],
            segmentStations,
            segmentStations.Count,
            connections.Count,
            estimatedMinutes,
            isTransferAfter);
    }
}