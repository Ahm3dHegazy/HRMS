namespace HRMS.Application.Features.Leave.Commands.ApproveLeaveRequest;

public sealed class ApproveLeaveRequestHandler(ILeaveRequestRepository leaveRequests, ILeaveBalanceRepository leaveBalances)
    : IRequestHandler<ApproveLeaveRequestCommand, Result<LeaveRequestReviewResult>>
{
    public async Task<Result<LeaveRequestReviewResult>> Handle(ApproveLeaveRequestCommand command, CancellationToken cancellationToken)
    {
        var leaveRequest = await leaveRequests.GetByIdAsync(command.Id);
        
        if (leaveRequest is null)
            return Result<LeaveRequestReviewResult>.Failure(Error.NotFound("Leave command not found"));

        if (leaveRequest.Status != LeaveStatus.Pending)
            return Result<LeaveRequestReviewResult>.Failure(
                Error.Conflict("Only pending leave requests can be approved."));

        var requestedDaysByYear = leaveRequest.GetRequestedDaysByYear();
        if (requestedDaysByYear.Count == 0)
            return Result<LeaveRequestReviewResult>.Failure(Error.Validation("The leave date range is invalid."));

        var balances = await leaveBalances.GetForEmployeeAndLeaveTypeAsync(
            leaveRequest.EmployeeId,
            leaveRequest.LeaveTypeId,
            requestedDaysByYear.Keys.Min(),
            requestedDaysByYear.Keys.Max(),
            cancellationToken);

        foreach (var (year, requestedDays) in requestedDaysByYear)
        {
            var balance = balances.SingleOrDefault(balance => balance.Year == year);
            if (balance is null)
                return Result<LeaveRequestReviewResult>.Failure(Error.Conflict(
                    $"No leave balance is configured for this employee and leave type in {year}."));

            if (!balance.CanDeduct(requestedDays))
                return Result<LeaveRequestReviewResult>.Failure(Error.Conflict(
                    $"Insufficient leave balance for {year}. Available: {balance.AvailableDays}, requested: {requestedDays}."));
        }

        foreach (var (year, requestedDays) in requestedDaysByYear)
        {
            var balance = balances.Single(balance => balance.Year == year);
            balance.TryDeduct(requestedDays);
        }

        if (!leaveRequest.Approve(DateTime.UtcNow))
            return Result<LeaveRequestReviewResult>.Failure(
                Error.Conflict("Only pending leave requests can be approved."));
        
        leaveRequests.Update(leaveRequest);
        await leaveRequests.SaveChangesAsync();

        return Result<LeaveRequestReviewResult>.Success(
            new LeaveRequestReviewResult(leaveRequest.Status.ToString(), leaveRequest.ReviewedAt!.Value));
    }
}

