namespace MetroNetwork.Application.Features.MetroLines.DTOs;

public sealed record MetroLineResponse(
    int Id,
    string Name,
    IReadOnlyList<MetroLineStationResponse> Stations);

public sealed record MetroLineStationResponse(
    int Id,
    string Name,
    int Sequence);