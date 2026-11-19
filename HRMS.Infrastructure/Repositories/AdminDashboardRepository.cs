namespace HRMS.Infrastructure.Repositories;

public sealed class AdminDashboardRepository(AppDbContext dbContext, HRMS.Application.Common.Attendance.IAttendanceClock clock) : IAdminDashboardRepository
{
    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var totalEmployees = await dbContext.Employees.CountAsync(cancellationToken);
        var activeEmployees = await dbContext.Employees
            .CountAsync(employee => employee.Status == EmploymentStatus.Active, cancellationToken);
        var employeesOnLeave = await dbContext.Employees
            .CountAsync(employee => employee.Status == EmploymentStatus.OnLeave, cancellationToken);
        var terminatedEmployees = await dbContext.Employees
            .CountAsync(employee => employee.Status == EmploymentStatus.Terminated, cancellationToken);
        var totalDepartments = await dbContext.Departments.CountAsync(cancellationToken);
        var pendingLeaveRequests = await dbContext.LeaveRequests
            .CountAsync(request => request.Status == LeaveStatus.Pending, cancellationToken);

        return new AdminDashboardDto(
            totalEmployees,
            activeEmployees,
            employeesOnLeave,
            terminatedEmployees,
            totalDepartments,
            pendingLeaveRequests);
    }

    public async Task<AdminDashboardAttendanceDto> GetAttendanceAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(clock.GetLocalNow().DateTime);
        var records = dbContext.AttendanceRecords.Where(record => record.Date == today);
        var total = await records.CountAsync(cancellationToken);
        var present = await records.CountAsync(record => record.Status == AttendanceStatus.Present, cancellationToken);
        var late = await records.CountAsync(record => record.Status == AttendanceStatus.Late, cancellationToken);
        var absent = await records.CountAsync(record => record.Status == AttendanceStatus.Absent, cancellationToken);
        var onLeave = await records.CountAsync(record => record.Status == AttendanceStatus.OnLeave, cancellationToken);

        return new AdminDashboardAttendanceDto(today, total, present, late, absent, onLeave);
    }

    public async Task<IReadOnlyList<AdminDashboardActivityDto>> GetRecentActivitiesAsync(
        CancellationToken cancellationToken)
    {
        var attendanceActivities = await dbContext.AttendanceRecords.AsNoTracking()
            .OrderByDescending(record => record.Date)
            .ThenByDescending(record => record.Id)
            .Take(10)
            .Select(record => new
            {
                record.Date,
                record.Status,
                EmployeeName = record.Employee.FullName
            })
            .ToListAsync(cancellationToken);

        var leaveActivities = await dbContext.LeaveRequests.AsNoTracking()
            .Where(request => request.ReviewedAt.HasValue)
            .OrderByDescending(request => request.ReviewedAt)
            .Take(10)
            .Select(request => new
            {
                OccurredAt = request.ReviewedAt!.Value,
                request.Status,
                EmployeeName = request.Employee.FullName
            })
            .ToListAsync(cancellationToken);

        return attendanceActivities
            .Select(activity => new AdminDashboardActivityDto(
                "Attendance",
                $"{activity.EmployeeName}: {activity.Status}",
                activity.Date.ToDateTime(TimeOnly.MinValue)))
            .Concat(leaveActivities.Select(activity => new AdminDashboardActivityDto(
                "LeaveRequest",
                $"{activity.EmployeeName}: leave request {activity.Status}",
                activity.OccurredAt)))
            .OrderByDescending(activity => activity.OccurredAt)
            .Take(10)
            .ToArray();
    }
}
