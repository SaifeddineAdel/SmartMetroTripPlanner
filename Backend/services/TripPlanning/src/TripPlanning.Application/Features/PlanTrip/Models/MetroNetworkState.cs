using TripPlanning.Application.Features.PlanTrip.DTOs;
using TripPlanning.Domain.Features.PlanTrip.Graph;

namespace TripPlanning.Application.Features.PlanTrip.Models;

public sealed class MetroNetworkState
{
    public string Version { get; }

    public MetroGraph Graph { get; }

    public IReadOnlyDictionary<int, StationData> StationsById { get; }

    public IReadOnlyDictionary<int, MetroLineData> MetroLinesById { get; }

    public MetroNetworkState(
        string version,
        MetroGraph graph,
        IReadOnlyDictionary<int, StationData> stationsById,
        IReadOnlyDictionary<int, MetroLineData> metroLinesById)
    {
        Version = version;
        Graph = graph;
        StationsById = stationsById;
        MetroLinesById = metroLinesById;
    }
}