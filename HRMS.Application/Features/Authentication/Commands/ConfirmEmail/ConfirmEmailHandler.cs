using HRMS.Application.Authentication;

namespace HRMS.Application.Features.Authentication.Commands.ConfirmEmail;

public sealed class ConfirmEmailHandler(IAuthService authService) 
    : IRequestHandler<ConfirmEmailCommand, Result<string>>
{
    public Task<Result<string>> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        return authService.ConfirmEmailAsync(command.UserId, command.Token, cancellationToken);
    }
}
