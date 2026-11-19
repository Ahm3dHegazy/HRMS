namespace HRMS.Application.Repositories;

public interface IPayrollRepository : IGenericRepository<Payroll>
{
    Task<Employee?> GetEmployeeForPayrollAsync(int employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Employee>> GetEligibleEmployeesAsync(DateOnly throughDate, CancellationToken cancellationToken);
    Task<IReadOnlyList<PayrollAttendanceDay>> GetAttendanceDaysAsync(int employeeId, DateOnly startDate, DateOnly endDateExclusive, CancellationToken cancellationToken);
    Task<IReadOnlyList<PayrollLeavePeriod>> GetApprovedLeavePeriodsAsync(int employeeId, DateOnly startDate, DateOnly endDateExclusive, CancellationToken cancellationToken);
    Task<Payroll?> GetForEmployeePeriodAsync(int employeeId, int year, int month, CancellationToken cancellationToken);
    Task<PayrollDto?> GetPayrollDtoByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<PayrollDto>> GetEmployeeHistoryAsync(int employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<PayrollDto>> GetMonthPayrollDtosAsync(int year, int month, CancellationToken cancellationToken);
    Task<bool> TryCreatePayrollAsync(Payroll payroll, CancellationToken cancellationToken);
    Task<bool> TryMarkPayrollPaidAsync(int payrollId, DateTime paidAt, CancellationToken cancellationToken);
}
