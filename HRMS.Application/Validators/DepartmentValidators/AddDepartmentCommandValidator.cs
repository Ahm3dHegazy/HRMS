namespace HRMS.Application.Validators.DepartmentValidators;

public sealed class AddDepartmentCommandValidator : AbstractValidator<AddDepartmentCommand>
{
    public AddDepartmentCommandValidator()
    {
        RuleFor(command => command.Name)
            .RequiredText("Department name", EntityLengthConstraints.OrganizationName);
        RuleFor(command => command.Description)
            .OptionalText("Department description", EntityLengthConstraints.OrganizationDescription);
        RuleFor(command => command.ManagerId)
            .GreaterThan(0).When(command => command.ManagerId.HasValue)
            .WithMessage("A valid manager ID is required.");
    }
}
