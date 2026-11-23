using FluentValidation;
using TripPlanning.Application.Features.PlanTrip.DTOs;

namespace TripPlanning.Application.Features.PlanTrip.Validators;

public sealed class PlanTripRequestValidator
    : AbstractValidator<PlanTripRequest>
{
    public PlanTripRequestValidator()
    {
        RuleFor(request => request.StartStationId)
            .GreaterThan(0)
            .WithMessage("Start station ID must be greater than zero.");

        RuleFor(request => request.DestinationStationId)
            .GreaterThan(0)
            .WithMessage("Destination station ID must be greater than zero.");
    }
}