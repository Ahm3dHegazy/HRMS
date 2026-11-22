namespace HRMS.Application.Features.Leave.Queries.GetLeaveRequestDetails;

public sealed record GetLeaveRequestDetailsQuery(int Id) : IRequest<Result<LeaveRequestDto>>;

