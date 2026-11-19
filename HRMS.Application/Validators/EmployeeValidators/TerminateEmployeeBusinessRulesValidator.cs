namespace HRMS.Application.Validators.EmployeeValidators;

public sealed class TerminateEmployeeBusinessRulesValidator : AbstractValidator<TerminateEmployeeCommand>
{
    public TerminateEmployeeBusinessRulesValidator(IEmployeeRepository employees)
    {
        RuleFor(command => command.TerminationDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The termination date cannot be in the future.");

        RuleFor(command => command.EmployeeId)
            .MustAsync(async (id, cancellationToken) =>
            {
                var employee = await employees.GetByIdAsync(id);
                return employee is null || employee.Status != EmploymentStatus.Terminated;
            })
            .When(command => command.EmployeeId > 0)
            .WithMessage("Employee is already terminated.")
            .WithErrorCode("Conflict");
    }
}
