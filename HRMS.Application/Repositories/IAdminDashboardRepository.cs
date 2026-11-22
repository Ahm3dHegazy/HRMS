namespace HRMS.Application.Repositories;

public interface IAdminDashboardRepository
{
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken);
    Task<AdminDashboardAttendanceDto> GetAttendanceAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AdminDashboardActivityDto>> GetRecentActivitiesAsync(CancellationToken cancellationToken);
    Task<AdminDashboardAttendanceTrendDto> GetAttendanceTrendAsync(int days, CancellationToken cancellationToken);
    Task<AdminDashboardPayrollSummaryDto> GetPayrollSummaryAsync(int? month, int? year, CancellationToken cancellationToken);
    Task<AdminDashboardLeaveSummaryDto> GetLeaveSummaryAsync(int? year, CancellationToken cancellationToken);
}
