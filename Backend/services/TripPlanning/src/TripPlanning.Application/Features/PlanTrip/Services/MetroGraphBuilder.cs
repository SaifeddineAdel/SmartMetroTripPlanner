using TripPlanning.Application.Common.DTOs;
using TripPlanning.Domain.Features.PlanTrip.Graph;

namespace TripPlanning.Application.Features.PlanTrip.Services;

public sealed class MetroGraphBuilder
{
    public MetroGraph Build(MetroNetworkSnapshot snapshot)
    {
        var graph = new MetroGraph();

        var groupedLineStations = snapshot.LineStations
            .GroupBy(lineStation => lineStation.MetroLineId);

        foreach (var lineGroup in groupedLineStations)
        {
            var orderedStations = lineGroup
                .OrderBy(lineStation => lineStation.Sequence)
                .ToList();

            for (var i = 0; i < orderedStations.Count - 1; i++)
            {
                var currentStation = orderedStations[i];
                var nextStation = orderedStations[i + 1];

                graph.AddConnection(
                    new GraphConnection(
                        currentStation.StationId,
                        nextStation.StationId,
                        currentStation.MetroLineId));

                graph.AddConnection(
                    new GraphConnection(
                        nextStation.StationId,
                        currentStation.StationId,
                        currentStation.MetroLineId));
            }
        }

        return graph;
    }
}