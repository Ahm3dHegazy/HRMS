using System.Linq.Expressions;

namespace HRMS.Infrastructure.Repositories;

public sealed class LeaveRequestRepository(AppDbContext dbContext)
    : GenericRepository<LeaveRequest>(dbContext), ILeaveRequestRepository
{

    public Task<bool> HasOverlappingPendingOrApprovedRequestAsync(int employeeId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken)
    {
        return dbContext.LeaveRequests.AnyAsync(leaveRequest =>
                leaveRequest.EmployeeId == employeeId &&
                (leaveRequest.Status == LeaveStatus.Pending || leaveRequest.Status == LeaveStatus.Approved) &&
                leaveRequest.StartDate <= endDate &&
                leaveRequest.EndDate >= startDate,
            cancellationToken);
    }

    public async Task<IReadOnlyList<LeaveRequestDto>> GetByStatusAsync(LeaveStatus status)
    {
        return await dbContext.LeaveRequests
            .Where(leaveRequest => leaveRequest.Status == status)
            .Select(ToDto())
            .ToListAsync();
    }

    public async Task<IReadOnlyList<LeaveRequestDto>> GetByEmployeeIdAsync(int employeeId)
    {
        return await dbContext.LeaveRequests
            .Where(leaveRequest => leaveRequest.EmployeeId == employeeId)
            .Select(ToDto())
            .ToListAsync();
    }

    public Task<LeaveRequestDto?> GetDetailsByIdAsync(int id)
    {
        return dbContext.LeaveRequests
            .Where(leaveRequest => leaveRequest.Id == id)
            .Select(ToDto())
            .SingleOrDefaultAsync();
    }

    private static Expression<Func<LeaveRequest, LeaveRequestDto>> ToDto()
    {
        return leaveRequest => new LeaveRequestDto(
            leaveRequest.Id,
            leaveRequest.EmployeeId,
            leaveRequest.LeaveType.Name,
            leaveRequest.StartDate,
            leaveRequest.EndDate,
            leaveRequest.Reason,
            leaveRequest.Status.ToString(),
            leaveRequest.ReviewedAt,
            leaveRequest.RejectionReason);
    }
}
