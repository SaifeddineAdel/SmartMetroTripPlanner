using FluentValidation;
using MetroNetwork.Application.Features.Stations.DTOs;

namespace MetroNetwork.Application.Features.Stations.Validators;

public sealed class CreateStationRequestValidator
    : AbstractValidator<CreateStationRequest>
{
    public CreateStationRequestValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Station name is required.")
            .MaximumLength(100)
            .WithMessage("Station name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Station description is required.")
            .MaximumLength(500)
            .WithMessage("Station description cannot exceed 500 characters.");
    }
}