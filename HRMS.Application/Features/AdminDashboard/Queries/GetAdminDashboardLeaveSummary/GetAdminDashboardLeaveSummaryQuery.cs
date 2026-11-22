namespace HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardLeaveSummary;

/// <summary>Year defaults to the current local year. Requests are counted by start date.</summary>
public sealed record GetAdminDashboardLeaveSummaryQuery(int? Year = null)
    : IRequest<AdminDashboardLeaveSummaryDto>;
