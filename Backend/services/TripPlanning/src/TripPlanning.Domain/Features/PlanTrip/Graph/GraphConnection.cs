namespace TripPlanning.Domain.Features.PlanTrip.Graph;

public sealed record GraphConnection(
    int FromStationId,
    int ToStationId,
    int MetroLineId);