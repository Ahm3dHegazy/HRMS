namespace HRMS.Application.Repositories;

public interface IAdminDashboardRepository
{
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken);
    Task<AdminDashboardAttendanceDto> GetAttendanceAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AdminDashboardActivityDto>> GetRecentActivitiesAsync(CancellationToken cancellationToken);
}
