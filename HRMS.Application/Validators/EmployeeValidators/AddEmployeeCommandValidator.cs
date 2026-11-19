namespace HRMS.Application.Validators.EmployeeValidators;

public sealed class AddEmployeeCommandValidator : AbstractValidator<AddEmployeeCommand>
{
    public AddEmployeeCommandValidator()
    {
        RuleFor(command => command.FullName)
            .RequiredText("Name", EntityLengthConstraints.EmployeeName);
        RuleFor(command => command.Address)
            .RequiredText("Address", EntityLengthConstraints.EmployeeAddress);
        RuleFor(command => command.Email)
            .EmailAddress(EntityLengthConstraints.EmployeeEmail);
        RuleFor(command => command.Phone)
            .PhoneNumber(EntityLengthConstraints.EmployeePhone);
        RuleFor(command => command.NationalId)
            .RequiredText("National ID", EntityLengthConstraints.EmployeeNationalId);
        RuleFor(command => command.NationalId)
            .Matches(@"^\d{14}$")
            .When(command => !string.IsNullOrWhiteSpace(command.NationalId))
            .WithMessage("National ID must contain exactly 14 digits.");

        RuleFor(command => command.DepartmentId).GreaterThan(0)
            .WithMessage("A valid department is required.");
        RuleFor(command => command.JobTitleId).GreaterThan(0)
            .WithMessage("A valid job title is required.");
        RuleFor(command => command.ManagerId).GreaterThan(0)
            .WithMessage("A valid manager ID is required.");
        RuleFor(command => command.HireDate).NotEqual(default(DateOnly))
            .WithMessage("Hire date is required.");
    }
}
