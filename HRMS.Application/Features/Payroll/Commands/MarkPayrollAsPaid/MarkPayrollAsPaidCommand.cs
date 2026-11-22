namespace HRMS.Application.Features.Payroll.Commands.MarkPayrollAsPaid;

public sealed record MarkPayrollAsPaidCommand(int PayrollId) : IRequest<Result<PayrollDto>>;
