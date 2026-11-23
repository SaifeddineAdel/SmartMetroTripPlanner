using FluentValidation;
using MetroNetwork.Application.Features.Stations.DTOs;

namespace MetroNetwork.Application.Features.Stations.Validators;

public sealed class UpdateStationDescriptionRequestValidator
    : AbstractValidator<UpdateStationDescriptionRequest>
{
    public UpdateStationDescriptionRequestValidator()
    {
        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Station description is required.")
            .MaximumLength(500)
            .WithMessage("Station description cannot exceed 500 characters.");
    }
}