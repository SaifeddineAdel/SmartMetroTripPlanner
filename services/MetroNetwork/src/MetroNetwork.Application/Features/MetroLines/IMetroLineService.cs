using MetroNetwork.Application.Features.MetroLines.DTOs;

namespace MetroNetwork.Application.Features.MetroLines;

public interface IMetroLineService
{
    Task<MetroLineResponse> CreateAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<MetroLineResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MetroLineResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task RenameAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
