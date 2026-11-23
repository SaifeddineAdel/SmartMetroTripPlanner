using FluentValidation;
using MetroNetwork.Application.Features.LineStations.DTOs;

namespace MetroNetwork.Application.Features.LineStations.Validators;

public sealed class CreateLineStationRequestValidator
    : AbstractValidator<CreateLineStationRequest>
{
    public CreateLineStationRequestValidator()
    {
        RuleFor(x => x.StationId)
            .GreaterThan(0)
            .WithMessage("Station ID must be greater than zero.");

        RuleFor(x => x.Sequence)
            .GreaterThan(0)
            .WithMessage("Sequence must be greater than zero.");
    }
}
