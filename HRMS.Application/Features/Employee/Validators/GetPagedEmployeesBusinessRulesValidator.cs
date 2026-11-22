namespace HRMS.Application.Features.Employee.Validators;

public sealed class GetPagedEmployeesBusinessRulesValidator : AbstractValidator<GetPagedEmployeesQuery>
{
    public GetPagedEmployeesBusinessRulesValidator(IDepartmentRepository departments)
    {
        RuleFor(query => query.EmployeeFilter.DepartmentId)
            .MustAsync(async (departmentId, cancellationToken) =>
                departmentId is null || await departments.GetByIdAsync(departmentId.Value) is not null)
            .When(query => query.EmployeeFilter.DepartmentId.HasValue)
            .WithMessage("The selected department does not exist.");
    }
}
