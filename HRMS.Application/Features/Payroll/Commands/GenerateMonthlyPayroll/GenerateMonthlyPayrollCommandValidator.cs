namespace HRMS.Application.Features.Payroll.Commands.GenerateMonthlyPayroll;

public sealed class GenerateMonthlyPayrollCommandValidator : AbstractValidator<GenerateMonthlyPayrollCommand>
{
    public GenerateMonthlyPayrollCommandValidator()
    {
        RuleFor(command => command.Month).InclusiveBetween(1, 12);
        RuleFor(command => command.Year).InclusiveBetween(1, 9999);
    }
}
