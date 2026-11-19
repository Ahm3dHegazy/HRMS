using HRMS.Application.Dtos.Authentication;
using HRMS.Application.Features.Authentication.Commands.Login;

namespace HRMS.Application.Validators.AuthenticationValidators;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
        => RuleFor(command => command.Request).SetValidator(new LoginDtoValidator());
}

