using TripPlanning.Application.Common.DTOs;

namespace TripPlanning.Application.Common.Abstractions;

public interface IMetroNetworkSnapshotReader
{
    Task<MetroNetworkSnapshot?> GetSnapshotAsync(
        CancellationToken cancellationToken = default);
}