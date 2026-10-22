namespace MetroNetwork.Application.Features.LineStations.DTOs;

public sealed record CreateLineStationRequest(
    int StationId,
    int Sequence);
