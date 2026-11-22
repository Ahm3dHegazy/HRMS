namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardPayrollSummary;

public sealed class GetAdminDashboardPayrollSummaryHandler(IAdminDashboardRepository repository)
    : IRequestHandler<GetAdminDashboardPayrollSummaryQuery, AdminDashboardPayrollSummaryDto>
{
    public Task<AdminDashboardPayrollSummaryDto> Handle(
        GetAdminDashboardPayrollSummaryQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetPayrollSummaryAsync(query.Month, query.Year, cancellationToken);
    }
}
