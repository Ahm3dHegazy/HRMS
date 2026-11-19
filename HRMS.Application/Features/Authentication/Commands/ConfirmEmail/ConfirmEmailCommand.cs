namespace HRMS.Application.Features.Authentication.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId, string Token) : IRequest<Result<string>>;
