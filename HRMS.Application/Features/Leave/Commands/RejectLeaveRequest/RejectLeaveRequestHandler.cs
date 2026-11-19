namespace HRMS.Application.Features.Leave.Commands.RejectLeaveRequest;

public sealed class RejectLeaveRequestHandler(ILeaveRequestRepository leaveRequests)
    : IRequestHandler<RejectLeaveRequestCommand, Result<LeaveRequestReviewResult>>
{
    public async Task<Result<LeaveRequestReviewResult>> Handle(RejectLeaveRequestCommand command,
        CancellationToken cancellationToken)
    {
        var leaveRequest = await leaveRequests.GetByIdAsync(command.Id);
        if (leaveRequest is null)
            return Result<LeaveRequestReviewResult>.Failure(Error.NotFound("Leave command not found"));

        if (!leaveRequest.Reject(command.RejectionReason, DateTime.UtcNow))
            return Result<LeaveRequestReviewResult>.Failure(
                Error.Conflict("Only pending leave requests can be rejected."));
        
        leaveRequests.Update(leaveRequest);
        await leaveRequests.SaveChangesAsync();

        return Result<LeaveRequestReviewResult>.Success(
            new LeaveRequestReviewResult(leaveRequest.Status.ToString(), leaveRequest.ReviewedAt!.Value));
    }
}

