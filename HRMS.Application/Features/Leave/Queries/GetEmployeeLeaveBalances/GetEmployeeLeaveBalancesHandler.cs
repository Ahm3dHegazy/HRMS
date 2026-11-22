namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveBalances;

public sealed class GetEmployeeLeaveBalancesHandler(ILeaveBalanceRepository leaveBalances)
    : IRequestHandler<GetEmployeeLeaveBalancesQuery, IReadOnlyList<LeaveBalanceDto>>
{
    public Task<IReadOnlyList<LeaveBalanceDto>> Handle(GetEmployeeLeaveBalancesQuery query,
        CancellationToken cancellationToken)
    {
        return leaveBalances.GetBalancesForEmployeeAsync(query.EmployeeId, cancellationToken);
    }
}
