using HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardAttendanceTrend;

namespace HRMS.Application.Features.AdminDashboard.Validators;

public sealed class GetAdminDashboardAttendanceTrendQueryValidator
    : AbstractValidator<GetAdminDashboardAttendanceTrendQuery>
{
    public GetAdminDashboardAttendanceTrendQueryValidator()
    {
        RuleFor(query => query.Days).InclusiveBetween(1, 365);
    }
}
