namespace HRMS.Infrastructure.Repositories;

public sealed class LeaveBalanceRepository(AppDbContext dbContext)
    : GenericRepository<LeaveBalance>(dbContext), ILeaveBalanceRepository
{
    public async Task<IReadOnlyList<LeaveBalance>> GetForEmployeeAndLeaveTypeAsync(int employeeId, int leaveTypeId,
        int startYear, int endYear, CancellationToken cancellationToken)
    {
        return await dbContext.LeaveBalances
            .Where(balance => balance.EmployeeId == employeeId && balance.LeaveTypeId == leaveTypeId &&
                              balance.Year >= startYear && balance.Year <= endYear)
            .OrderBy(balance => balance.Year)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LeaveBalanceDto>> GetBalancesForEmployeeAsync(int employeeId,
        CancellationToken cancellationToken)
    {
        return await dbContext.LeaveBalances
            .AsNoTracking()
            .Where(balance => balance.EmployeeId == employeeId)
            .OrderBy(balance => balance.Year)
            .ThenBy(balance => balance.LeaveType.Name)
            .Select(balance => new LeaveBalanceDto(
                balance.LeaveTypeId,
                balance.LeaveType.Name,
                balance.Year,
                balance.TotalDays,
                balance.UsedDays,
                balance.TotalDays - balance.UsedDays))
            .ToListAsync(cancellationToken);
    }
}
