namespace HRMS.Application.Features.Employee.Validators;

public class GetPagedEmployeesQueryValidator : AbstractValidator<GetPagedEmployeesQuery>
{
    public GetPagedEmployeesQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .NotEmpty().WithMessage("PageNumber cannot be empty")
            .GreaterThan(0).WithMessage("PageNumber cannot less than zero");

        RuleFor(query => query.PageSize)
            .NotEmpty().WithMessage("PageSize cannot be empty")
            .GreaterThan(0).WithMessage("PageSize cannot less than zero");

        RuleFor(query => query.EmployeeFilter.DepartmentId)
            .GreaterThan(0).When(query => query.EmployeeFilter.DepartmentId.HasValue)
            .WithMessage("DepartmentId must be greater than zero.");

        RuleFor(query => query.EmployeeFilter.Status)
            .Must(status => status is null || Enum.IsDefined(status.Value))
            .WithMessage("A valid employee status is required.");

        RuleFor(query => query.EmployeeFilter.Search)
            .MaximumLength(200)
            .WithMessage("Search cannot exceed 200 characters.");
    }
}

