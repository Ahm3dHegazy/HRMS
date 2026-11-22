namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveBalances;

public sealed record GetEmployeeLeaveBalancesQuery(int EmployeeId)
    : IRequest<IReadOnlyList<LeaveBalanceDto>>;
