namespace HRMS.Application.Repositories;

public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
}
