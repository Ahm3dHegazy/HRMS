namespace HRMS.Application.Features.Authentication.Commands.Login;

public sealed record LoginCommand(LoginDto Request) : IRequest<Result<string>>;

