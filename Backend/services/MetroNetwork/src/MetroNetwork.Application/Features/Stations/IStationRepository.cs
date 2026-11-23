using MetroNetwork.Application.Common.DTOs;
using MetroNetwork.Application.Features.Stations.DTOs;
using MetroNetwork.Domain.Features.Stations;

namespace MetroNetwork.Application.Features.Stations;

public interface IStationRepository
{
    Task<Station?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<StationResponse>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Station station,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);


    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<Station?> GetByIdWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default);
}
