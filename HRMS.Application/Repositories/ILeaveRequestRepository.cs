namespace HRMS.Application.Repositories;

public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
{
    Task<bool> HasOverlappingPendingOrApprovedRequestAsync(int employeeId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<LeaveRequestDto>> GetByStatusAsync(LeaveStatus status);
    Task<IReadOnlyList<LeaveRequestDto>> GetByEmployeeIdAsync(int employeeId);
    Task<LeaveRequestDto?> GetDetailsByIdAsync(int id);
}
