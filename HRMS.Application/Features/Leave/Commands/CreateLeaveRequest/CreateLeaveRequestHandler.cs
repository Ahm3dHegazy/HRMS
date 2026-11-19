namespace HRMS.Application.Features.Leave.Commands.CreateLeaveRequest;

public sealed class CreateLeaveRequestHandler(
    ILeaveRequestRepository leaveRequests,
    ILeaveBalanceRepository leaveBalances,
    IMapper mapper)
    : IRequestHandler<CreateLeaveRequestCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateLeaveRequestCommand command, CancellationToken cancellationToken)
    {
        var hasOverlappingRequest = await leaveRequests.HasOverlappingPendingOrApprovedRequestAsync(
            command.EmployeeId,
            command.StartDate,
            command.EndDate,
            cancellationToken);

        if (hasOverlappingRequest)
            return Result<int>.Failure(Error.Conflict(
                "The employee already has a pending or approved leave command for these dates."));

        var leaveRequest = mapper.Map<LeaveRequest>(command);
        var requestedDaysByYear = leaveRequest.GetRequestedDaysByYear();
        if (requestedDaysByYear.Count == 0)
            return Result<int>.Failure(Error.Validation("The leave date range is invalid."));

        var balances = await leaveBalances.GetForEmployeeAndLeaveTypeAsync(
            command.EmployeeId,
            command.LeaveTypeId,
            requestedDaysByYear.Keys.Min(),
            requestedDaysByYear.Keys.Max(),
            cancellationToken);

        foreach (var (year, requestedDays) in requestedDaysByYear)
        {
            var balance = balances.SingleOrDefault(balance => balance.Year == year);
            if (balance is null)
                return Result<int>.Failure(Error.Conflict(
                    $"No leave balance is configured for this employee and leave type in {year}."));

            if (!balance.CanDeduct(requestedDays))
                return Result<int>.Failure(Error.Conflict(
                    $"Insufficient leave balance for {year}. Available: {balance.AvailableDays}, requested: {requestedDays}."));
        }

        
        leaveRequest.Submit();

        await leaveRequests.AddAsync(leaveRequest);
        await leaveRequests.SaveChangesAsync();

        return Result<int>.Success(leaveRequest.Id);
    }
}

