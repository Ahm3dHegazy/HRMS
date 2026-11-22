namespace HRMS.Infrastructure.Repositories;

public sealed class AdminDashboardRepository(AppDbContext dbContext, HRMS.Application.Common.Attendance.IAttendanceClock clock) : IAdminDashboardRepository
{
    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var employeesByStatus = await dbContext.Employees
            .GroupBy(employee => employee.Status)
            .Select(group => new
            {
                Status = group.Key, 
                Count = group.Count()
            })
            .ToDictionaryAsync(item => 
                item.Status, item => item.Count, cancellationToken);

        var totalDepartments = await dbContext.Departments.CountAsync(cancellationToken);
        var pendingLeaveRequests = await dbContext.LeaveRequests
            .CountAsync(request => request.Status == LeaveStatus.Pending, cancellationToken);

        return new AdminDashboardDto(
            employeesByStatus.Values.Sum(),
            employeesByStatus.GetValueOrDefault(EmploymentStatus.Active),
            employeesByStatus.GetValueOrDefault(EmploymentStatus.OnLeave),
            employeesByStatus.GetValueOrDefault(EmploymentStatus.Terminated),
            totalDepartments,
            pendingLeaveRequests);
    }

    public async Task<AdminDashboardAttendanceDto> GetAttendanceAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(clock.GetLocalNow().DateTime);

        var byStatus = await dbContext.AttendanceRecords
            .Where(record => record.Date == today)
            .GroupBy(record => record.Status)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToDictionaryAsync(item => item.Status, item => item.Count, cancellationToken);

        return new AdminDashboardAttendanceDto(
            today,
            byStatus.Values.Sum(),
            byStatus.GetValueOrDefault(AttendanceStatus.Present),
            byStatus.GetValueOrDefault(AttendanceStatus.Late),
            byStatus.GetValueOrDefault(AttendanceStatus.Absent),
            byStatus.GetValueOrDefault(AttendanceStatus.OnLeave));
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

    public async Task<AdminDashboardAttendanceTrendDto> GetAttendanceTrendAsync(
        int days,
        CancellationToken cancellationToken)
    {
        var to = DateOnly.FromDateTime(clock.GetLocalNow().DateTime);
        var from = to.AddDays(-(days - 1));

        var counts = await dbContext.AttendanceRecords
            .Where(record => record.Date >= from && record.Date <= to)
            .GroupBy(record => new { record.Date, record.Status })
            .Select(group => new { group.Key.Date, group.Key.Status, Count = group.Count() })
            .ToListAsync(cancellationToken);

        var countsByDate = counts.ToLookup(item => item.Date);
        var points = new List<AdminDashboardAttendanceTrendPointDto>(days);

        for (var date = from; date <= to; date = date.AddDays(1))
        {
            var byStatus = countsByDate[date].ToDictionary(item => item.Status, item => item.Count);
            points.Add(new AdminDashboardAttendanceTrendPointDto(
                date,
                byStatus.GetValueOrDefault(AttendanceStatus.Present),
                byStatus.GetValueOrDefault(AttendanceStatus.Late),
                byStatus.GetValueOrDefault(AttendanceStatus.Absent),
                byStatus.GetValueOrDefault(AttendanceStatus.OnLeave)));
        }

        return new AdminDashboardAttendanceTrendDto(from, to, points);
    }

    public async Task<AdminDashboardPayrollSummaryDto> GetPayrollSummaryAsync(
        int? month,
        int? year,
        CancellationToken cancellationToken)
    {
        var now = clock.GetLocalNow();
        var resolvedMonth = month ?? now.Month;
        var resolvedYear = year ?? now.Year;

        // At most two rows (Paid / Unpaid) come back; they are combined in memory.
        var groups = await dbContext.Payrolls
            .Where(payroll => payroll.Month == resolvedMonth && payroll.Year == resolvedYear)
            .GroupBy(payroll => payroll.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count(),
                BaseSalary = group.Sum(payroll => payroll.BaseSalary),
                AbsenceDeduction = group.Sum(payroll => payroll.AbsenceDeduction),
                LateDeduction = group.Sum(payroll => payroll.LateDeduction),
                UnpaidLeaveDeduction = group.Sum(payroll => payroll.UnpaidLeaveDeduction),
                NetSalary = group.Sum(payroll => payroll.NetSalary)
            })
            .ToListAsync(cancellationToken);

        var paid = groups.FirstOrDefault(group => group.Status == PayrollStatus.Paid);
        var unpaid = groups.FirstOrDefault(group => group.Status == PayrollStatus.Unpaid);

        return new AdminDashboardPayrollSummaryDto(
            resolvedMonth,
            resolvedYear,
            groups.Sum(group => group.Count),
            paid?.Count ?? 0,
            unpaid?.Count ?? 0,
            groups.Sum(group => group.BaseSalary),
            groups.Sum(group => group.AbsenceDeduction),
            groups.Sum(group => group.LateDeduction),
            groups.Sum(group => group.UnpaidLeaveDeduction),
            groups.Sum(group => group.NetSalary),
            paid?.NetSalary ?? 0m,
            unpaid?.NetSalary ?? 0m);
    }

    public async Task<AdminDashboardLeaveSummaryDto> GetLeaveSummaryAsync(
        int? year,
        CancellationToken cancellationToken)
    {
        var resolvedYear = year ?? clock.GetLocalNow().Year;
        var yearStart = new DateOnly(resolvedYear, 1, 1);
        var yearEnd = new DateOnly(resolvedYear, 12, 31);

        // Requests are attributed to the year in which they start.
        var counts = await dbContext.LeaveRequests
            .Where(request => request.StartDate >= yearStart && request.StartDate <= yearEnd)
            .GroupBy(request => new
            {
                request.LeaveTypeId,
                LeaveTypeName = request.LeaveType.Name,
                request.Status
            })
            .Select(group => new
            {
                group.Key.LeaveTypeId,
                group.Key.LeaveTypeName,
                group.Key.Status,
                Count = group.Count()
            })
            .ToListAsync(cancellationToken);

        int CountFor(IEnumerable<(LeaveStatus Status, int Count)> rows, LeaveStatus status) =>
            rows.Where(row => row.Status == status).Sum(row => row.Count);

        var byLeaveType = counts
            .GroupBy(item => new { item.LeaveTypeId, item.LeaveTypeName })
            .Select(group =>
            {
                var rows = group.Select(item => (item.Status, item.Count)).ToList();
                return new AdminDashboardLeaveTypeSummaryDto(
                    group.Key.LeaveTypeId,
                    group.Key.LeaveTypeName,
                    rows.Sum(row => row.Count),
                    CountFor(rows, LeaveStatus.Pending),
                    CountFor(rows, LeaveStatus.Approved),
                    CountFor(rows, LeaveStatus.Rejected),
                    CountFor(rows, LeaveStatus.Cancelled));
            })
            .OrderBy(summary => summary.LeaveTypeName)
            .ToList();

        return new AdminDashboardLeaveSummaryDto(
            resolvedYear,
            byLeaveType.Sum(summary => summary.Total),
            byLeaveType.Sum(summary => summary.Pending),
            byLeaveType.Sum(summary => summary.Approved),
            byLeaveType.Sum(summary => summary.Rejected),
            byLeaveType.Sum(summary => summary.Cancelled),
            byLeaveType);
    }
}
