namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardRecentActivities;

public sealed class GetAdminDashboardRecentActivitiesHandler(IAdminDashboardRepository repository)
    : IRequestHandler<GetAdminDashboardRecentActivitiesQuery, IReadOnlyList<AdminDashboardActivityDto>>
{
    public Task<IReadOnlyList<AdminDashboardActivityDto>> Handle(
        GetAdminDashboardRecentActivitiesQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetRecentActivitiesAsync(cancellationToken);
    }
}

