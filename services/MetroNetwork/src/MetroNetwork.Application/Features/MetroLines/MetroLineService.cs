using MetroNetwork.Application.Common.Exceptions;
using MetroNetwork.Application.Features.MetroLines.DTOs;
using MetroNetwork.Domain.Features.MetroLines;

namespace MetroNetwork.Application.Features.MetroLines;

public sealed class MetroLineService : IMetroLineService
{
    private readonly IMetroLineRepository _repository;

    public MetroLineService(IMetroLineRepository repository)
    {
        _repository = repository;
    }

    public async Task<MetroLineResponse> CreateAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        await EnsureMetroLineNameIsUniqueAsync(
            name,
            cancellationToken);

        var metroLine = MetroLine.Create(name);

        await _repository.AddAsync(metroLine, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(metroLine);
    }

    public async Task<MetroLineResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var metroLine = await GetRequiredMetroLineAsync(
            id,
            cancellationToken);

        return MapToResponse(metroLine);
    }

    public async Task<IReadOnlyList<MetroLineResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var metroLines = await _repository.GetAllAsync(cancellationToken);

        return metroLines
            .Select(MapToResponse)
            .ToList();
    }

    public async Task RenameAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var metroLine = await GetRequiredMetroLineAsync(
            id,
            cancellationToken);

        if (IsSameName(metroLine.Name, newName))
        {
            throw new ConflictException(
                "The new metro line name is the same as the current name.");
        }

        await EnsureMetroLineNameIsUniqueAsync(
            newName,
            cancellationToken);

        metroLine.Rename(newName);

        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var metroLine = await GetRequiredMetroLineAsync(
            id,
            cancellationToken);

        var hasStations = await _repository.HasStationsAsync(
            id,
            cancellationToken);

        if (hasStations)
        {
            throw new ConflictException(
                "Cannot delete a metro line that has stations assigned to it.");
        }

        await _repository.DeleteAsync(metroLine, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private async Task<MetroLine> GetRequiredMetroLineAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var metroLine = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        return metroLine ?? throw new NotFoundException(
            $"Metro line with ID '{id}' was not found.");
    }

    private async Task EnsureMetroLineNameIsUniqueAsync(
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsByNameAsync(
            name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"A metro line with the name '{name}' already exists.");
        }
    }

    private static bool IsSameName(
        string currentName,
        string newName)
    {
        return string.Equals(
            currentName,
            newName?.Trim(),
            StringComparison.OrdinalIgnoreCase);
    }

    private static MetroLineResponse MapToResponse(MetroLine metroLine)
    {
        return new MetroLineResponse(
            metroLine.Id,
            metroLine.Name);
    }
}
