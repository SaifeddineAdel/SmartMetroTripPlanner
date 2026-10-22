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

        var queue = new Queue<int>();
        var visited = new HashSet<int> { startStationId };
        var previousConnections = new Dictionary<int, GraphConnection>();

        queue.Enqueue(startStationId);

        while (queue.Count > 0)
        {
            var currentStationId = queue.Dequeue();

            foreach (var connection in graph.GetConnectionsFrom(currentStationId))
            {
                if (!visited.Add(connection.ToStationId))
                    continue;

                previousConnections[connection.ToStationId] = connection;

                if (connection.ToStationId == destinationStationId)
                {
                    return ReconstructPath(
                        previousConnections,
                        startStationId,
                        destinationStationId);
                }

                queue.Enqueue(connection.ToStationId);
            }
        }

        return null;
    }

    private static IReadOnlyList<GraphConnection> ReconstructPath(
        Dictionary<int, GraphConnection> previousConnections,
        int startStationId,
        int destinationStationId)
    {
        var path = new List<GraphConnection>();
        var currentStationId = destinationStationId;

        while (currentStationId != startStationId)
        {
            var connection = previousConnections[currentStationId];

            path.Add(connection);
            currentStationId = connection.FromStationId;
        }

        path.Reverse();

        return path;
    }
}