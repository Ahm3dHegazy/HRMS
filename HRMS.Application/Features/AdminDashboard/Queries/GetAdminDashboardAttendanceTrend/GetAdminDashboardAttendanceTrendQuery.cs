namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardAttendanceTrend;

public sealed record GetAdminDashboardAttendanceTrendQuery(int Days = 30)
    : IRequest<AdminDashboardAttendanceTrendDto>;
