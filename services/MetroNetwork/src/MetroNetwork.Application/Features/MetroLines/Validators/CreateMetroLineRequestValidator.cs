using FluentValidation;
using MetroNetwork.Application.Features.MetroLines.DTOs;

namespace MetroNetwork.Application.Features.MetroLines.Validators;

public sealed class CreateMetroLineRequestValidator
    : AbstractValidator<CreateMetroLineRequest>
{
    public CreateMetroLineRequestValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Metro line name is required.")
            .MaximumLength(100)
            .WithMessage("Metro line name cannot exceed 100 characters.");
    }
}
