using MetroNetwork.Application.Common.Exceptions;
using MetroNetwork.Application.Features.LineStations.DTOs;
using MetroNetwork.Domain.Features.LineStations;
using MetroNetwork.Domain.Common.Exceptions;

namespace MetroNetwork.Application.Features.LineStations;

public sealed class LineStationService : ILineStationService
{
    private readonly ILineStationRepository _repository;

    public LineStationService(ILineStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<LineStationResponse> AddStationToLineAsync(
        int metroLineId,
        CreateLineStationRequest request,
        CancellationToken cancellationToken = default)
    {
        await EnsureMetroLineExistsAsync(
            metroLineId,
            cancellationToken);

        await EnsureStationExistsAsync(
            request.StationId,
            cancellationToken);

        var exists = await _repository.ExistsAsync(
            metroLineId,
            request.StationId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "This station is already assigned to this metro line.");
        }

        await EnsureSequenceIsAvailableAsync(
            metroLineId,
            request.Sequence,
            cancellationToken);

        var lineStation = LineStation.Create(
            metroLineId,
            request.StationId,
            request.Sequence);

        await _repository.AddAsync(lineStation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(lineStation);
    }

    public async Task<IReadOnlyList<LineStationResponse>> GetStationsByLineAsync(
        int metroLineId,
        CancellationToken cancellationToken = default)
    {
        await EnsureMetroLineExistsAsync(
            metroLineId,
            cancellationToken);

        var lineStations = await _repository.GetByMetroLineIdAsync(
            metroLineId,
            cancellationToken);

        return lineStations
            .Select(MapToResponse)
            .ToList();
    }

    public async Task ChangeSequenceAsync(
        int metroLineId,
        int stationId,
        int newSequence,
        CancellationToken cancellationToken = default)
    {
        var lineStation = await GetRequiredLineStationAsync(
            metroLineId,
            stationId,
            cancellationToken);

        if (lineStation.Sequence == newSequence)
        {
            throw new ConflictException(
                "The new sequence is the same as the current sequence.");
        }

        await EnsureSequenceIsAvailableAsync(
            metroLineId,
            newSequence,
            cancellationToken);

        lineStation.ChangeSequence(newSequence);

        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveStationFromLineAsync(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken = default)
    {
        var lineStation = await GetRequiredLineStationAsync(
            metroLineId,
            stationId,
            cancellationToken);

        _repository.Remove(lineStation);

        await _repository.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureMetroLineExistsAsync(
        int metroLineId,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.MetroLineExistsAsync(
            metroLineId,
            cancellationToken);

        if (!exists)
        {
            throw new NotFoundException(
                $"Metro line with ID '{metroLineId}' was not found.");
        }
    }

    private async Task EnsureStationExistsAsync(
        int stationId,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.StationExistsAsync(
            stationId,
            cancellationToken);

        if (!exists)
        {
            throw new NotFoundException(
                $"Station with ID '{stationId}' was not found or has been deleted.");
        }
    }

    private async Task EnsureSequenceIsAvailableAsync(
        int metroLineId,
        int sequence,
        CancellationToken cancellationToken)
    {
        if (sequence <= 0)
        {
            throw new DomainValidationException(
                "Sequence must be greater than zero.");
        }

        var exists = await _repository.SequenceExistsAsync(
            metroLineId,
            sequence,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Sequence '{sequence}' is already used in this metro line.");
        }
    }

    private async Task<LineStation> GetRequiredLineStationAsync(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken)
    {
        var lineStation = await _repository.GetAsync(
            metroLineId,
            stationId,
            cancellationToken);

        return lineStation ?? throw new NotFoundException(
            $"Station '{stationId}' is not assigned to metro line '{metroLineId}'.");
    }

    private static LineStationResponse MapToResponse(
        LineStation lineStation)
    {
        return new LineStationResponse(
            lineStation.MetroLineId,
            lineStation.StationId,
            lineStation.Sequence);
    }
}
