namespace HRMS.Application.Repositories;

public interface ILeaveBalanceRepository : IGenericRepository<LeaveBalance>
{
    Task<IReadOnlyList<LeaveBalance>> GetForEmployeeAndLeaveTypeAsync(int employeeId, int leaveTypeId,
        int startYear, int endYear, CancellationToken cancellationToken);
    Task<IReadOnlyList<LeaveBalanceDto>> GetBalancesForEmployeeAsync(int employeeId,
        CancellationToken cancellationToken);
}
