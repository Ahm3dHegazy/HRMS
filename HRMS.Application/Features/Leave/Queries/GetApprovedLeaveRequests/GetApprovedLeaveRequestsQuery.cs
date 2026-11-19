namespace HRMS.Application.Features.Leave.Queries.GetApprovedLeaveRequests;

public sealed record GetApprovedLeaveRequestsQuery : IRequest<IReadOnlyList<ApprovedLeaveRequestDto>>;

