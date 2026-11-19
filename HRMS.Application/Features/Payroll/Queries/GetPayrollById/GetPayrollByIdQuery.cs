namespace HRMS.Application.Features.Payroll.Queries.GetPayrollById;

public sealed record GetPayrollByIdQuery(int Id) : IRequest<Result<PayrollDto>>;
