using HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardLeaveSummary;

namespace HRMS.Application.Features.AdminDashboard.Validators;

public sealed class GetAdminDashboardLeaveSummaryQueryValidator
    : AbstractValidator<GetAdminDashboardLeaveSummaryQuery>
{
    public GetAdminDashboardLeaveSummaryQueryValidator()
    {
        RuleFor(query => query.Year)
            .Must(year => year is >= 1 and <= 9998)
            .When(query => query.Year.HasValue)
            .WithMessage("Year must be between 1 and 9998.");
    }
}
