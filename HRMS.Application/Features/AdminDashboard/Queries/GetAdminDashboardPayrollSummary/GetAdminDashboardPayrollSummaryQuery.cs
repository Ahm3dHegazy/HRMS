namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardPayrollSummary;

/// <summary>Month and Year default to the current local month when omitted.</summary>
public sealed record GetAdminDashboardPayrollSummaryQuery(int? Month = null, int? Year = null)
    : IRequest<AdminDashboardPayrollSummaryDto>;
