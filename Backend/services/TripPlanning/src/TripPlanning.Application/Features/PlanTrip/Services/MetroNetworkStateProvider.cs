using TripPlanning.Application.Common.Abstractions;
using TripPlanning.Application.Common.Exceptions;
using TripPlanning.Application.Features.PlanTrip.DTOs;
using TripPlanning.Application.Features.PlanTrip.Models;

namespace TripPlanning.Application.Features.PlanTrip.Services;

public sealed class MetroNetworkStateProvider
{
    private readonly IMetroNetworkSnapshotReader _snapshotReader;
    private readonly MetroGraphBuilder _graphBuilder;

    private MetroNetworkState? _currentState;

    public MetroNetworkStateProvider(
        IMetroNetworkSnapshotReader snapshotReader,
        MetroGraphBuilder graphBuilder)
    {
        _snapshotReader = snapshotReader;
        _graphBuilder = graphBuilder;
    }

    public MetroNetworkState GetCurrentState()
    {
        var currentState = Volatile.Read(ref _currentState);

        if (currentState is null)
        {
            throw new ServiceUnavailableException(
                "Metro network is not initialized yet. Please try again later.");
        }

        return currentState;
    }

    public async Task<bool> RefreshAsync(
        CancellationToken cancellationToken = default)
    {
        var snapshot = await _snapshotReader.GetSnapshotAsync(
            cancellationToken);

        if (snapshot is null)
        {
            return false;
        }

        var currentState = Volatile.Read(ref _currentState);

        if (currentState?.Version == snapshot.Version)
        {
            return false;
        }

        var graph = _graphBuilder.Build(snapshot);

        var stationsById = snapshot.Stations.ToDictionary(
            station => station.Id,
            station => new StationData(
                station.Id,
                station.Name));

        var metroLinesById = snapshot.MetroLines.ToDictionary(
            line => line.Id,
            line => new MetroLineData(
                line.Id,
                line.Name));

        var newState = new MetroNetworkState(
            snapshot.Version,
            graph,
            stationsById,
            metroLinesById);

        Interlocked.Exchange(
            ref _currentState,
            newState);

        return true;
    }
}