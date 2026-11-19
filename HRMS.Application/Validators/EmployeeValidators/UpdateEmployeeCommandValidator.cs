namespace HRMS.Application.Validators.EmployeeValidators;

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(command => command.EmployeeId).GreaterThan(0)
            .WithMessage("A valid employee ID is required.");
        RuleFor(command => command.Address)
            .RequiredText("Address", EntityLengthConstraints.EmployeeAddress);
        RuleFor(command => command.Email)
            .EmailAddress(EntityLengthConstraints.EmployeeEmail);
        RuleFor(command => command.Phone)
            .PhoneNumber(EntityLengthConstraints.EmployeePhone);
        RuleFor(command => command.DepartmentId).GreaterThan(0)
            .WithMessage("A valid department is required.");
        RuleFor(command => command.JobTitleId).GreaterThan(0)
            .WithMessage("A valid job title is required.");
        RuleFor(command => command.ManagerId).GreaterThan(0)
            .When(command => command.ManagerId.HasValue)
            .WithMessage("A valid manager ID is required.");
    }
}
