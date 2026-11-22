namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardLeaveSummary;

public sealed class GetAdminDashboardLeaveSummaryHandler(IAdminDashboardRepository repository)
    : IRequestHandler<GetAdminDashboardLeaveSummaryQuery, AdminDashboardLeaveSummaryDto>
{
    public Task<AdminDashboardLeaveSummaryDto> Handle(
        GetAdminDashboardLeaveSummaryQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetLeaveSummaryAsync(query.Year, cancellationToken);
    }
}
