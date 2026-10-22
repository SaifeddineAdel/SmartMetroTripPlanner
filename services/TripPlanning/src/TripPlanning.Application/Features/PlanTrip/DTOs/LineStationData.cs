namespace TripPlanning.Application.Features.PlanTrip.DTOs;

public sealed record LineStationData(
    int MetroLineId,
    int StationId,
    int Sequence);