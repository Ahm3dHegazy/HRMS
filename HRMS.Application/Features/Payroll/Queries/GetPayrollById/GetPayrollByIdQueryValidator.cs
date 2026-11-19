namespace HRMS.Application.Features.Payroll.Queries.GetPayrollById;

public sealed class GetPayrollByIdQueryValidator : AbstractValidator<GetPayrollByIdQuery>
{
    public GetPayrollByIdQueryValidator() => RuleFor(query => query.Id).GreaterThan(0);
}
