namespace HRMS.Application.Features.Department.Validators;

public sealed class GetPagedDepartmentsQueryValidator : AbstractValidator<GetPagedDepartmentsQuery>
{
    public GetPagedDepartmentsQueryValidator()
    {
        RuleFor(query => query.PageNumber).GreaterThan(0);
        RuleFor(query => query.PageSize).GreaterThan(0);
    }
}
