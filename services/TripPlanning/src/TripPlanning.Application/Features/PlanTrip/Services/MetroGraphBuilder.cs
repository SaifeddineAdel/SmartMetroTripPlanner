using TripPlanning.Application.Features.PlanTrip.Abstractions;
using TripPlanning.Domain.Features.PlanTrip.Graph;

namespace TripPlanning.Application.Features.PlanTrip.Services;

public sealed class MetroGraphBuilder
{
    private readonly IMetroNetworkClient _metroNetworkClient;

    public MetroGraphBuilder(IMetroNetworkClient metroNetworkClient)
    {
        _metroNetworkClient = metroNetworkClient;
    }

    public async Task<MetroGraph> BuildAsync(
        CancellationToken cancellationToken)
    {
        var lines = await _metroNetworkClient.GetMetroLinesAsync(
            cancellationToken);

        var graph = new MetroGraph();

        foreach (var line in lines)
        {
            var lineStations =
                await _metroNetworkClient.GetLineStationsAsync(
                    line.Id,
                    cancellationToken);

            var orderedStations = lineStations
                .OrderBy(station => station.Sequence)
                .ToList();

            for (var i = 0; i < orderedStations.Count - 1; i++)
            {
                var currentStation = orderedStations[i];
                var nextStation = orderedStations[i + 1];

                graph.AddConnection(
                    new GraphConnection(
                        currentStation.StationId,
                        nextStation.StationId,
                        line.Id));

                graph.AddConnection(
                    new GraphConnection(
                        nextStation.StationId,
                        currentStation.StationId,
                        line.Id));
            }
        }

        return graph;
    }
}