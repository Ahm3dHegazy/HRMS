using HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardPayrollSummary;

namespace HRMS.Application.Features.AdminDashboard.Validators;

public sealed class GetAdminDashboardPayrollSummaryQueryValidator
    : AbstractValidator<GetAdminDashboardPayrollSummaryQuery>
{
    public GetAdminDashboardPayrollSummaryQueryValidator()
    {
        RuleFor(query => query.Month)
            .Must(month => month is >= 1 and <= 12)
            .When(query => query.Month.HasValue)
            .WithMessage("Month must be between 1 and 12.");

        RuleFor(query => query.Year)
            .Must(year => year is >= 1 and <= 9998)
            .When(query => query.Year.HasValue)
            .WithMessage("Year must be between 1 and 9998.");
    }
}
