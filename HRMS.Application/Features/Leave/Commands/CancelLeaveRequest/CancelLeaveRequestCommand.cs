namespace HRMS.Application.Features.Leave.Commands.CancelLeaveRequest;

public sealed record CancelLeaveRequestCommand(int Id) : IRequest<Result<string>>;

