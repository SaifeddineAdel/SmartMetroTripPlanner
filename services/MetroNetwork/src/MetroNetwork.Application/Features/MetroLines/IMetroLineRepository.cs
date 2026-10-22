using MetroNetwork.Domain.Features.MetroLines;

namespace MetroNetwork.Application.Features.MetroLines;

public interface IMetroLineRepository
{
    Task<MetroLine?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MetroLine>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<bool> HasStationsAsync(
        int metroLineId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        MetroLine metroLine,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        MetroLine metroLine,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
