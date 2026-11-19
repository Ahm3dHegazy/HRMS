namespace HRMS.Application.Features.Leave.Commands.ApproveLeaveRequest;

public sealed record ApproveLeaveRequestCommand(int Id) : IRequest<Result<LeaveRequestReviewResult>>;

