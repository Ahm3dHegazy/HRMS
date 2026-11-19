namespace HRMS.Application.Features.Leave.Queries.GetPendingLeaveRequests;

public sealed record GetPendingLeaveRequestsQuery : IRequest<IReadOnlyList<PendingLeaveRequestDto>>;

