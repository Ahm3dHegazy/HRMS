namespace HRMS.Application.Features.Payroll.Queries.GetEmployeePayrollHistory;

public sealed record GetEmployeePayrollHistoryQuery(int EmployeeId) : IRequest<Result<IReadOnlyList<PayrollDto>>>;
