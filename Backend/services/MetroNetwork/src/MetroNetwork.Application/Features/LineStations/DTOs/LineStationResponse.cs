namespace MetroNetwork.Application.Features.LineStations.DTOs;

public sealed record LineStationResponse(
    int MetroLineId,
    int StationId,
    int Sequence);
