using FluentValidation;
using MetroNetwork.Application.Features.LineStations.DTOs;

namespace MetroNetwork.Application.Features.LineStations.Validators;

public sealed class ChangeLineStationSequenceRequestValidator
    : AbstractValidator<ChangeLineStationSequenceRequest>
{
    public ChangeLineStationSequenceRequestValidator()
    {
        RuleFor(x => x.Sequence)
            .GreaterThan(0)
            .WithMessage("Sequence must be greater than zero.");
    }
}
