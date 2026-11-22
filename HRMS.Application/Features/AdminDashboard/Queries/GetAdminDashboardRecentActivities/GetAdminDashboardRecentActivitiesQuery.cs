namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardRecentActivities;

public sealed record GetAdminDashboardRecentActivitiesQuery : IRequest<IReadOnlyList<AdminDashboardActivityDto>>;

