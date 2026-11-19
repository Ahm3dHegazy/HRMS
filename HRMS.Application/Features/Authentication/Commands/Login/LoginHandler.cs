using HRMS.Application.Authentication;

namespace HRMS.Application.Features.Authentication.Commands.Login;

public sealed class LoginHandler(IAuthService authService) : IRequestHandler<LoginCommand, Result<string>>
{
    public Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        return authService.LoginAsync(command.Request, cancellationToken);
    }
}

