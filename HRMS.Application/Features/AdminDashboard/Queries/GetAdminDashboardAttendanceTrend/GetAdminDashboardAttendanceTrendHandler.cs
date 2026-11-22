namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardAttendanceTrend;

public sealed class GetAdminDashboardAttendanceTrendHandler(IAdminDashboardRepository repository)
    : IRequestHandler<GetAdminDashboardAttendanceTrendQuery, AdminDashboardAttendanceTrendDto>
{
    public Task<AdminDashboardAttendanceTrendDto> Handle(
        GetAdminDashboardAttendanceTrendQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetAttendanceTrendAsync(query.Days, cancellationToken);
    }
}
