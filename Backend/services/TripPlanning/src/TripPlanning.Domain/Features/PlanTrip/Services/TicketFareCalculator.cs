namespace TripPlanning.Domain.Features.PlanTrip.Services;

public sealed class TicketFareCalculator
{
    public decimal Calculate(int totalStationsCount)
    {
        return totalStationsCount switch
        {
            <= 9 => 10m,
            <= 16 => 12m,
            <= 23 => 15m,
            _ => 20m
        };
    }
}