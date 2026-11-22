namespace HRMS.Application.Features.Leave.Queries.GetLeaveRequestDetails;

public sealed class GetLeaveRequestDetailsHandler(ILeaveRequestRepository leaveRequests)
    : IRequestHandler<GetLeaveRequestDetailsQuery, Result<LeaveRequestDto>>
{
    public async Task<Result<LeaveRequestDto>> Handle(GetLeaveRequestDetailsQuery query, CancellationToken cancellationToken)
    {
        var leaveRequest = await leaveRequests.GetDetailsByIdAsync(query.Id);
        
        return leaveRequest is null
            ? Result<LeaveRequestDto>.Failure(Error.NotFound("Leave query not found"))
            : Result<LeaveRequestDto>.Success(leaveRequest);
    }
}

