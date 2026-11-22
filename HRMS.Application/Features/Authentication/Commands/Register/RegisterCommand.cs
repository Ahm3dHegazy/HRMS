namespace HRMS.Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand(RegisterDto Request) : IRequest<Result<string>>;

