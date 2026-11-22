namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveBalances;

public sealed class GetEmployeeLeaveBalancesQueryValidator : AbstractValidator<GetEmployeeLeaveBalancesQuery>
{
    public GetEmployeeLeaveBalancesQueryValidator()
    {
        RuleFor(query => query.EmployeeId).GreaterThan(0);
    }
}
