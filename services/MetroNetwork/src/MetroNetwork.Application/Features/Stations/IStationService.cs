using MetroNetwork.Application.Features.Stations.DTOs;
using MetroNetwork.Application.Common.DTOs;

namespace MetroNetwork.Application.Features.Stations;

public interface IStationService
{
    Task<StationResponse> CreateAsync(
        string name,
        string description,
        CancellationToken cancellationToken = default);

    Task<StationResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<StationResponse>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task RenameAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task UpdateDescriptionAsync(
        int id,
        string description,
        CancellationToken cancellationToken = default);
}