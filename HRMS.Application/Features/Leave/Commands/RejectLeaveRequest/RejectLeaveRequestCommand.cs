namespace HRMS.Application.Features.Leave.Commands.RejectLeaveRequest;

public sealed record RejectLeaveRequestCommand(int Id, string RejectionReason)
    : IRequest<Result<LeaveRequestReviewResult>>;

