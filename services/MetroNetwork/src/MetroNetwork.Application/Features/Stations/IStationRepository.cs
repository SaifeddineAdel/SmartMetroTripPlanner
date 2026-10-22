using MetroNetwork.Domain.Features.Stations;

namespace MetroNetwork.Application.Features.Stations;

public interface IStationRepository
{
    Task<Station?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Station>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Station station,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);


    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}
