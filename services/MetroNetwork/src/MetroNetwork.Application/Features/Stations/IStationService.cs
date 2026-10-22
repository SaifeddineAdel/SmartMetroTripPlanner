using MetroNetwork.Application.Features.Stations.DTOs;

namespace MetroNetwork.Application.Features.Stations;

public interface IStationService
{
    Task<StationResponse> CreateAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<StationResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StationResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task RenameAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}