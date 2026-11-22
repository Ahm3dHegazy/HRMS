namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboard;

public sealed class GetAdminDashboardHandler(IAdminDashboardRepository repository)
    : IRequestHandler<GetAdminDashboardQuery, AdminDashboardDto>
{
    public Task<AdminDashboardDto> Handle(GetAdminDashboardQuery query, CancellationToken cancellationToken)
    {
        return repository.GetDashboardAsync(cancellationToken);
    }
}

