namespace TripPlanning.Application.Features.PlanTrip.DTOs;

public sealed record PlanTripRequest(
    int StartStationId,
    int DestinationStationId);