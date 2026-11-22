namespace HRMS.Application.Features.Payroll.Commands.GeneratePayroll;

public sealed record GeneratePayrollCommand(int EmployeeId, int Month, int Year) 
    : IRequest<Result<PayrollDto>>;
