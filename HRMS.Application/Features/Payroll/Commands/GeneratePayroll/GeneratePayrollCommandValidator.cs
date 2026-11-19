namespace HRMS.Application.Features.Payroll.Commands.GeneratePayroll;

public sealed class GeneratePayrollCommandValidator : AbstractValidator<GeneratePayrollCommand>
{
    public GeneratePayrollCommandValidator()
    {
        RuleFor(command => command.EmployeeId).GreaterThan(0);
        RuleFor(command => command.Month).InclusiveBetween(1, 12);
        RuleFor(command => command.Year).InclusiveBetween(1, 9999);
    }
}
