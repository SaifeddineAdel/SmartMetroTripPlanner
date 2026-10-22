using TripPlanning.Application.Common.Exceptions;
using TripPlanning.Application.Features.PlanTrip.Abstractions;
using TripPlanning.Application.Features.PlanTrip.DTOs;
using TripPlanning.Domain.Features.PlanTrip.Graph;
using TripPlanning.Domain.Features.PlanTrip.Services;

namespace TripPlanning.Application.Features.PlanTrip.Services;

public sealed class PlanTripService
{
    private readonly IMetroNetworkClient _metroNetworkClient;
    private readonly MetroGraphBuilder _graphBuilder;
    private readonly MetroPathFinder _pathFinder;

    public PlanTripService(
        IMetroNetworkClient metroNetworkClient,
        MetroGraphBuilder graphBuilder,
        MetroPathFinder pathFinder)
    {
        _metroNetworkClient = metroNetworkClient;
        _graphBuilder = graphBuilder;
        _pathFinder = pathFinder;
    }

    public async Task<TripPlanResponse> PlanAsync(
        PlanTripRequest request,
        CancellationToken cancellationToken)
    {
        var stations = await _metroNetworkClient
            .GetStationsAsync(cancellationToken);

        var start = stations.FirstOrDefault(
            station => station.Id == request.StartStationId);

        var destination = stations.FirstOrDefault(
            station => station.Id == request.DestinationStationId);

        if (start is null || destination is null)
        {
            throw new NotFoundException(
                "The start station or destination station was not found.");
        }

        var lines = await _metroNetworkClient
            .GetMetroLinesAsync(cancellationToken);

        var stationsById = stations.ToDictionary(s => s.Id);
        var linesById = lines.ToDictionary(l => l.Id);

        var graph = await _graphBuilder.BuildAsync(cancellationToken);

        var path = _pathFinder.FindShortestPath(
            graph,
            start.Id,
            destination.Id);

        if (path is null)
        {
            throw new NotFoundException(
                "No route exists between the selected stations.");
        }

        var segments = BuildSegments(path, stationsById, linesById);

        var totalStationsCount = path.Count + 1;

        var totalMinutes = path.Sum(connection =>
            connection.MetroLineId == 1 ? 2.5 : 2.0);

        var fare = totalStationsCount switch
        {
            <= 9 => 10m,
            <= 16 => 12m,
            <= 23 => 15m,
            _ => 20m
        };

        return new TripPlanResponse(
            start,
            destination,
            segments,
            totalStationsCount,
            totalMinutes,
            fare);
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