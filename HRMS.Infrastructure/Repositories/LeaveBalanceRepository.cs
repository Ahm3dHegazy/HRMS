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
}
