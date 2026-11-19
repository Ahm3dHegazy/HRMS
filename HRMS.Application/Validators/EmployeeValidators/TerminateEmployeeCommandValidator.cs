namespace HRMS.Application.Validators.EmployeeValidators;

public sealed class TerminateEmployeeCommandValidator : AbstractValidator<TerminateEmployeeCommand>
{
    public TerminateEmployeeCommandValidator()
    {
        RuleFor(command => command.TerminationType)
            .IsInEnum().WithMessage("A valid termination type is required.");

        RuleFor(command => command.Reason)
            .RequiredText("Termination reason", EntityLengthConstraints.EmployeeTerminationReason);

        RuleFor(command => command.EmployeeId).GreaterThan(0)
            .WithMessage("A valid employee ID is required.");
        RuleFor(command => command.TerminationDate).NotEqual(default(DateOnly))
            .WithMessage("A termination date is required.");
    }
}
