namespace TripPlanning.Application.Features.PlanTrip.DTOs;

public sealed record TripPlanResponse(
    StationData StartStation,
    StationData DestinationStation,
    IReadOnlyList<TripSegmentResponse> Segments,
    int TotalStationCount,
    double TotalEstimatedMinutes,
    decimal Fare);

public sealed record TripSegmentResponse(
    MetroLineData MetroLine,
    IReadOnlyList<StationData> Stations,
    int StationCount,
    int TransitionCount,
    double EstimatedMinutes,
    bool IsTransferAfter);