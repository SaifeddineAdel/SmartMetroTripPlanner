using MetroNetwork.Application.Common.Exceptions;
using MetroNetwork.Application.Features.Stations.DTOs;
using MetroNetwork.Domain.Features.Stations;

namespace MetroNetwork.Application.Features.Stations;

public sealed class StationService : IStationService
{
    private readonly IStationRepository _repository;

    public StationService(IStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<StationResponse> CreateAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        await EnsureStationNameIsUniqueAsync(
            name,
            cancellationToken);

        var station = Station.Create(name);

        await _repository.AddAsync(
            station,
            cancellationToken);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(station);
    }

    public async Task<StationResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var station = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (station is null)
        {
            throw new NotFoundException(
                $"Station with ID '{id}' was not found.");
        }

        return MapToResponse(station);
    }

    public async Task<IReadOnlyList<StationResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var stations = await _repository.GetAllAsync(
            cancellationToken);

        return stations
            .Select(MapToResponse)
            .ToList();
    }

    public async Task RenameAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var station = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (station is null)
        {
            throw new NotFoundException(
                $"Station with ID '{id}' was not found.");
        }

        if (IsSameName(station.Name, newName))
        {
            throw new ConflictException(
                    "The new station name is the same as the current name.");
        }

        await EnsureStationNameIsUniqueAsync(
            newName,
            cancellationToken);

        station.Rename(newName);

        await _repository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var station = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (station is null)
        {
            throw new NotFoundException(
                $"Station with ID '{id}' was not found.");
        }

        station.Delete();

        await _repository.SaveChangesAsync(
            cancellationToken);
    }

    private async Task EnsureStationNameIsUniqueAsync(
        string name,
        CancellationToken cancellationToken)
    {
        var existsStation = await _repository.ExistsByNameAsync(
            name,
            cancellationToken);

        if (existsStation)
        {
            throw new ConflictException(
                $"A station with the name '{name}' already exists.");
        }
    }

    private static bool IsSameName(
        string currentName,
        string newName)
    {
        return string.Equals(
            currentName,
            newName.Trim(),
            StringComparison.OrdinalIgnoreCase);
    }

    private static StationResponse MapToResponse(Station station)
    {
        return new StationResponse(
            station.Id,
            station.Name);
    }
}
