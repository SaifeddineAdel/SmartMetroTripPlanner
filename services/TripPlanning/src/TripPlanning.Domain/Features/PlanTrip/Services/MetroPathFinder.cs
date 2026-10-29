
using TripPlanning.Domain.Features.PlanTrip.Graph;

namespace TripPlanning.Domain.Features.PlanTrip.Services;

public sealed class MetroPathFinder
{
    public IReadOnlyList<GraphConnection>? FindShortestPath(
        MetroGraph graph,
        int startStationId,
        int destinationStationId)
    {
        if (startStationId == destinationStationId)
            return Array.Empty<GraphConnection>();

        var startState = new SearchState(startStationId, null);

        var queue = new PriorityQueue<SearchState, (int Stations, int Transfers)>();

        var distances = new Dictionary<SearchState, (int Stations, int Transfers)>
        {
            [startState] = (0, 0)
        };

        var previousStates =
            new Dictionary<SearchState, (SearchState Previous, GraphConnection Connection)>();

        queue.Enqueue(startState, (0, 0));

        while (queue.Count > 0)
        {
            queue.TryDequeue(out var currentState, out var currentPriority);

            if (!distances.TryGetValue(currentState, out var knownPriority)
                || currentPriority != knownPriority)
            {
                continue;
            }

            if (currentState.StationId == destinationStationId)
            {
                return ReconstructPath(
                    previousStates,
                    startState,
                    currentState);
            }

            foreach (var connection in graph.GetConnectionsFrom(currentState.StationId))
            {
                var transferCost =
                    currentState.LineId.HasValue
                    && currentState.LineId.Value != connection.MetroLineId
                        ? 1
                        : 0;

                var nextState = new SearchState(
                    connection.ToStationId,
                    connection.MetroLineId);

                var nextPriority = (
                    Stations: currentPriority.Stations + 1,
                    Transfers: currentPriority.Transfers + transferCost
                );

                if (distances.TryGetValue(nextState, out var existingPriority)
                    && ComparePriority(existingPriority, nextPriority) <= 0)
                {
                    continue;
                }

                distances[nextState] = nextPriority;

                previousStates[nextState] = (currentState, connection);

                queue.Enqueue(nextState, nextPriority);
            }
        }

        return null;
    }

    private static int ComparePriority(
        (int Stations, int Transfers) first,
        (int Stations, int Transfers) second)
    {
        var stationComparison = first.Stations.CompareTo(second.Stations);

        if (stationComparison != 0)
            return stationComparison;

        return first.Transfers.CompareTo(second.Transfers);
    }

    private static IReadOnlyList<GraphConnection> ReconstructPath(
        Dictionary<SearchState, (SearchState Previous, GraphConnection Connection)> previousStates,
        SearchState startState,
        SearchState destinationState)
    {
        var path = new List<GraphConnection>();
        var currentState = destinationState;

        while (currentState != startState)
        {
            var previous = previousStates[currentState];

            path.Add(previous.Connection);
            currentState = previous.Previous;
        }

        path.Reverse();

        return path;
    }

    private readonly record struct SearchState(
        int StationId,
        int? LineId);
}
