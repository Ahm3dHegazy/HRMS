namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveRequests;

public sealed class GetEmployeeLeaveRequestsHandler(ILeaveRequestRepository leaveRequests)
    : IRequestHandler<GetEmployeeLeaveRequestsQuery, IReadOnlyList<LeaveRequestDto>>
{
    public async Task<IReadOnlyList<LeaveRequestDto>> Handle(GetEmployeeLeaveRequestsQuery query,
        CancellationToken cancellationToken)
    {
        return await leaveRequests.GetByEmployeeIdAsync(query.EmployeeId);
    }
}

