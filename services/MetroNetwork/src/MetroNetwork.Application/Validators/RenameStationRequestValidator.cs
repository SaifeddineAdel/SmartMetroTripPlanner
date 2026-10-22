using FluentValidation;
using MetroNetwork.Application.Features.Stations.DTOs;

namespace MetroNetwork.Application.Features.Stations.Validators;

public sealed class RenameStationRequestValidator
    : AbstractValidator<RenameStationRequest>
{
    public RenameStationRequestValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Station name is required.")
            .MaximumLength(100)
            .WithMessage("Station name cannot exceed 100 characters.");
    }
}
