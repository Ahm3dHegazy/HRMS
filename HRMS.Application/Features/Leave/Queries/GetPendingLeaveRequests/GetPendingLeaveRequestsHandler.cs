namespace HRMS.Application.Features.Leave.Queries.GetPendingLeaveRequests;

public sealed class GetPendingLeaveRequestsHandler(ILeaveRequestRepository leaveRequests)
    : IRequestHandler<GetPendingLeaveRequestsQuery, IReadOnlyList<PendingLeaveRequestDto>>
{
    public async Task<IReadOnlyList<PendingLeaveRequestDto>> Handle(GetPendingLeaveRequestsQuery query,
        CancellationToken cancellationToken)
    {
        var requests = await leaveRequests.GetByStatusAsync(LeaveStatus.Pending);
        return requests.Select(requestDto => new PendingLeaveRequestDto(requestDto)).ToArray();
    }
}

