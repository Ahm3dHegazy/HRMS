namespace HRMS.Application.Features.Authentication.Validators;

public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}

