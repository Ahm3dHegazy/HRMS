namespace HRMS.Application.Features.Payroll.Commands.GenerateMonthlyPayroll;

public sealed record GenerateMonthlyPayrollCommand(int Month, int Year) : IRequest<Result<IReadOnlyList<PayrollDto>>>;
