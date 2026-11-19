using HRMS.Application.Dtos.Authentication;
using HRMS.Application.Features.Authentication.Commands.Register;

namespace HRMS.Application.Validators.AuthenticationValidators;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
        => RuleFor(command => command.Request).SetValidator(new RegisterDtoValidator());
}

