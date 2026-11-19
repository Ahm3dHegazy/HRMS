using HRMS.Application.Authentication;

namespace HRMS.Application.Features.Authentication.Commands.Register;

public sealed class RegisterHandler(IAuthService authService) : IRequestHandler<RegisterCommand, Result<string>>
{
    public Task<Result<string>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return authService.RegisterAsync(command.Request, cancellationToken);
    }
}

