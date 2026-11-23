namespace TripPlanning.Application.Common.DTOs;

public sealed class MetroNetworkSnapshot
{
    public string Version { get; set; } = string.Empty;

    public DateTimeOffset PublishedAtUtc { get; set; }

    public List<StationSnapshot> Stations { get; set; } = [];

    public List<MetroLineSnapshot> MetroLines { get; set; } = [];

    public List<LineStationSnapshot> LineStations { get; set; } = [];
}

public sealed record StationSnapshot(
    int Id,
    string Name);

public sealed record MetroLineSnapshot(
    int Id,
    string Name);

public sealed record LineStationSnapshot(
    int MetroLineId,
    int StationId,
    int Sequence);