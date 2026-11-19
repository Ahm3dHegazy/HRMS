namespace HRMS.Application.Features.Leave.Queries.GetRejectedLeaveRequests;

public sealed record GetRejectedLeaveRequestsQuery : IRequest<IReadOnlyList<RejectedLeaveRequestDto>>;

