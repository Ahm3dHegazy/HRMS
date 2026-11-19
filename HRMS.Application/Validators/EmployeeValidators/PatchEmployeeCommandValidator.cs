namespace HRMS.Application.Validators.EmployeeValidators;

public sealed class PatchEmployeeCommandValidator : AbstractValidator<PatchEmployeeCommand>
{
    public PatchEmployeeCommandValidator()
    {
        RuleFor(command => command.EmployeeId).GreaterThan(0)
            .WithMessage("A valid employee ID is required.");

        RuleFor(command => command)
            .Must(command => command.Address is not null || command.Email is not null || command.Phone is not null ||
                             command.BaseSalary.HasValue || command.DepartmentId.HasValue || command.JobTitleId.HasValue ||
                             command.ManagerId.HasValue || command.ClearManagerId)
            .WithMessage("At least one employee field must be provided.");

        RuleFor(command => command.Address)
            .RequiredText("Address", EntityLengthConstraints.EmployeeAddress)
            .When(command => command.Address is not null);
        RuleFor(command => command.Email)
            .EmailAddress(EntityLengthConstraints.EmployeeEmail)
            .When(command => command.Email is not null);
        RuleFor(command => command.Phone)
            .PhoneNumber(EntityLengthConstraints.EmployeePhone)
            .When(command => command.Phone is not null);

        RuleFor(command => command.DepartmentId).GreaterThan(0)
            .When(command => command.DepartmentId.HasValue)
            .WithMessage("A valid department is required.");
        RuleFor(command => command.JobTitleId).GreaterThan(0)
            .When(command => command.JobTitleId.HasValue)
            .WithMessage("A valid job title is required.");
        RuleFor(command => command.ManagerId).GreaterThan(0)
            .When(command => command.ManagerId.HasValue)
            .WithMessage("A valid manager ID is required.");

        RuleFor(command => command.ClearManagerId)
            .Equal(false).When(command => command.ManagerId.HasValue)
            .WithMessage("Provide ManagerId or ClearManagerId, not both.");
    }
}
