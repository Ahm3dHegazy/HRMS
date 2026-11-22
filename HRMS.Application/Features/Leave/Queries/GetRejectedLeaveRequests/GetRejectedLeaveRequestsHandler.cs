namespace HRMS.Application.Features.Leave.Queries.GetRejectedLeaveRequests;

public sealed class GetRejectedLeaveRequestsHandler(ILeaveRequestRepository leaveRequests)
    : IRequestHandler<GetRejectedLeaveRequestsQuery, IReadOnlyList<RejectedLeaveRequestDto>>
{
    public async Task<IReadOnlyList<RejectedLeaveRequestDto>> Handle(GetRejectedLeaveRequestsQuery query,
        CancellationToken cancellationToken)
    {
        var requests = await leaveRequests.GetByStatusAsync(LeaveStatus.Rejected);
        return requests.Select(requestDto => new RejectedLeaveRequestDto(requestDto)).ToArray();
    }
}

