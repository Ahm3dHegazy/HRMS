using HRMS.Application.Dtos.Payroll;

namespace HRMS.Infrastructure.Repositories;

public sealed class PayrollRepository(AppDbContext dbContext) : GenericRepository<Payroll>(dbContext), IPayrollRepository
{
    public Task<Employee?> GetEmployeeForPayrollAsync(int employeeId, CancellationToken cancellationToken)
    {
        return dbContext.Employees.AsNoTracking()
            .SingleOrDefaultAsync(employee => employee.Id == employeeId, cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> GetEligibleEmployeesAsync(DateOnly throughDate,
        CancellationToken cancellationToken)
    {
        return await dbContext.Employees.AsNoTracking()
            .Where(employee => employee.Status != EmploymentStatus.Terminated && employee.HireDate <= throughDate)
            .OrderBy(employee => employee.Id).ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<PayrollAttendanceDay>> GetAttendanceDaysAsync(int employeeId, DateOnly startDate,
        DateOnly endDateExclusive, CancellationToken cancellationToken)
    {
        return await dbContext.AttendanceRecords.AsNoTracking()
            .Where(record =>
                record.EmployeeId == employeeId && record.Date >= startDate && record.Date < endDateExclusive)
            .Select(record => new PayrollAttendanceDay(record.Date, record.Status))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PayrollLeavePeriod>> GetApprovedLeavePeriodsAsync(int employeeId,
        DateOnly startDate,
        DateOnly endDateExclusive, CancellationToken cancellationToken)
    {
        return await dbContext.LeaveRequests.AsNoTracking()
            .Where(request => request.EmployeeId == employeeId && request.Status == LeaveStatus.Approved
                                                               && request.StartDate < endDateExclusive &&
                                                               request.EndDate >= startDate &&
                                                               !request.LeaveType.IsPaid)
            .Select(request => new PayrollLeavePeriod(request.StartDate, request.EndDate))
            .ToListAsync(cancellationToken);
    }

    public Task<Payroll?> GetForEmployeePeriodAsync(int employeeId, int year, int month,
        CancellationToken cancellationToken)
    {
        return dbContext.Payrolls.SingleOrDefaultAsync(payroll =>
            payroll.EmployeeId == employeeId && payroll.Year == year && payroll.Month == month, cancellationToken);
    }

    public Task<PayrollDto?> GetPayrollDtoByIdAsync(int id, CancellationToken cancellationToken)
    {
        return ProjectPayrolls(dbContext.Payrolls.AsNoTracking().Where(payroll => payroll.Id == id))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PayrollDto>> GetEmployeeHistoryAsync(int employeeId,
        CancellationToken cancellationToken)
    {
        return
            await ProjectPayrolls(dbContext.Payrolls.AsNoTracking().Where(payroll => payroll.EmployeeId == employeeId))
                .OrderByDescending(payroll => payroll.Year).ThenByDescending(payroll => payroll.Month)
                .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PayrollDto>> GetMonthPayrollDtosAsync(int year, int month,
        CancellationToken cancellationToken)
    {
        return
            await ProjectPayrolls(dbContext.Payrolls.AsNoTracking()
                    .Where(payroll => payroll.Year == year && payroll.Month == month))
                .OrderBy(payroll => payroll.EmployeeId).ToListAsync(cancellationToken);
    }

    public async Task<bool> TryCreatePayrollAsync(Payroll payroll, CancellationToken cancellationToken)
    {
        await dbContext.Payrolls.AddAsync(payroll, cancellationToken);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (exception.GetBaseException() is Microsoft.Data.SqlClient.SqlException { Number: 2601 or 2627 })
        {
            dbContext.Entry(payroll).State = EntityState.Detached;
            return false;
        }
    }

    public async Task<bool> TryMarkPayrollPaidAsync(int payrollId, DateTime paidAt, CancellationToken cancellationToken)
    {
        return
            await dbContext.Payrolls.Where(payroll => payroll.Id == payrollId && payroll.Status == PayrollStatus.Unpaid)
                .ExecuteUpdateAsync(setters => setters.SetProperty(payroll => payroll.Status, PayrollStatus.Paid)
                    .SetProperty(payroll => payroll.PaidAt, (DateTime?)paidAt), cancellationToken) == 1;
    }

    private static IQueryable<PayrollDto> ProjectPayrolls(IQueryable<Payroll> query)
    {
        return query.Select(payroll =>
            new PayrollDto(payroll.Id, payroll.EmployeeId, payroll.Employee.FullName, payroll.Month, payroll.Year,
                payroll.BaseSalary, payroll.AbsenceDeduction, payroll.LateDeduction, payroll.UnpaidLeaveDeduction,
                payroll.NetSalary, payroll.Status.ToString(), payroll.CreatedAt, payroll.PaidAt));
    }
}
