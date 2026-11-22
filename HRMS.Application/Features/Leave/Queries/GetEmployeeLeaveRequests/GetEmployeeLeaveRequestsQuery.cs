namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveRequests;

public sealed record GetEmployeeLeaveRequestsQuery(int EmployeeId)
    : IRequest<IReadOnlyList<LeaveRequestDto>>;

