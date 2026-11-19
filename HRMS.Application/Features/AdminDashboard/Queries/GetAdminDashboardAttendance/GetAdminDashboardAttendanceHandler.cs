namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardAttendance;

public sealed class GetAdminDashboardAttendanceHandler(IAdminDashboardRepository repository)
    : IRequestHandler<GetAdminDashboardAttendanceQuery, AdminDashboardAttendanceDto>
{
    public Task<AdminDashboardAttendanceDto> Handle(
        GetAdminDashboardAttendanceQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetAttendanceAsync(cancellationToken);
    }
}

