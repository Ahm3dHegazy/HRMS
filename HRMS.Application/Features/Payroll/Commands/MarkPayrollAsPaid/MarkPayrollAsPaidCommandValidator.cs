namespace HRMS.Application.Features.Payroll.Commands.MarkPayrollAsPaid;

public sealed class MarkPayrollAsPaidCommandValidator : AbstractValidator<MarkPayrollAsPaidCommand>
{
    public MarkPayrollAsPaidCommandValidator() => RuleFor(command => command.PayrollId).GreaterThan(0);
}
