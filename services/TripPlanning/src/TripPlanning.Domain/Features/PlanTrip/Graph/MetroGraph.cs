namespace TripPlanning.Domain.Features.PlanTrip.Graph;

public sealed class MetroGraph
{
    private readonly List<GraphConnection> _connections = [];

    public IReadOnlyList<GraphConnection> Connections => _connections;

    public void AddConnection(GraphConnection connection)
    {
        _connections.Add(connection);
    }

    public IReadOnlyList<GraphConnection> GetConnectionsFrom(int stationId)
    {
        return _connections
            .Where(connection => connection.FromStationId == stationId)
            .ToList();
    }
}