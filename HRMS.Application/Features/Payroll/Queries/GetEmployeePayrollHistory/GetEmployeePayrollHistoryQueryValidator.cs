namespace HRMS.Application.Features.Payroll.Queries.GetEmployeePayrollHistory;

public sealed class GetEmployeePayrollHistoryQueryValidator : AbstractValidator<GetEmployeePayrollHistoryQuery>
{
    public GetEmployeePayrollHistoryQueryValidator() => RuleFor(query => query.EmployeeId).GreaterThan(0);
}
