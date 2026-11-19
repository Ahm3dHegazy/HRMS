namespace HRMS.Application.Features.Leave.Commands.CancelLeaveRequest;

public sealed class CancelLeaveRequestHandler(ILeaveRequestRepository leaveRequests)
    : IRequestHandler<CancelLeaveRequestCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CancelLeaveRequestCommand command, CancellationToken cancellationToken)
    {
        var leaveRequest = await leaveRequests.GetByIdAsync(command.Id);
        if (leaveRequest is null)
            return Result<string>.Failure(Error.NotFound("Leave command not found"));

        if (!leaveRequest.Cancel())
            return Result<string>.Failure(
                Error.Conflict("Only pending leave requests can be cancelled."));
        leaveRequests.Update(leaveRequest);
        await leaveRequests.SaveChangesAsync();

        return Result<string>.Success(leaveRequest.Status.ToString());
    }
}

