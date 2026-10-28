using FluentValidation;
using MetroNetwork.Application.Features.Auth.DTOs;

namespace MetroNetwork.Application.Features.Auth.Validators;

public sealed class RevokeTokenRequestValidator
    : AbstractValidator<RevokeTokenRequest>
{
    public RevokeTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.")
            .MaximumLength(512)
            .WithMessage("Refresh token must not exceed 512 characters.");
    }
}