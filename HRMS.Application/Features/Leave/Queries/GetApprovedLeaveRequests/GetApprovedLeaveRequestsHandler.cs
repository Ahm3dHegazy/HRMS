namespace HRMS.Application.Features.Leave.Queries.GetApprovedLeaveRequests;

public sealed class GetApprovedLeaveRequestsHandler(ILeaveRequestRepository leaveRequests)
    : IRequestHandler<GetApprovedLeaveRequestsQuery, IReadOnlyList<ApprovedLeaveRequestDto>>
{
    public async Task<IReadOnlyList<ApprovedLeaveRequestDto>> Handle(GetApprovedLeaveRequestsQuery query,
        CancellationToken cancellationToken)
    {
        var requests = await leaveRequests.GetByStatusAsync(LeaveStatus.Approved);
        return requests.Select(requestDto => new ApprovedLeaveRequestDto(requestDto)).ToArray();
    }
}

