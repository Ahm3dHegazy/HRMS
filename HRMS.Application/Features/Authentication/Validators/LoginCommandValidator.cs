using HRMS.Application.Dtos.Authentication;
using HRMS.Application.Features.Authentication.Commands.Login;

namespace HRMS.Application.Features.Authentication.Validators;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
        => RuleFor(command => command.Request).SetValidator(new LoginDtoValidator());
}

