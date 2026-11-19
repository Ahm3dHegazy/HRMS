namespace HRMS.Application.Features.Payroll;

internal static class PayrollGeneration
{
    public static (DateOnly Start, DateOnly EndExclusive) Period(int year, int month)
    {
        var start = new DateOnly(year, month, 1);
        return (start, start.AddMonths(1));
    }

    public static Error? ValidateSettings(int? workingDays) => workingDays is > 0 and <= 31
        ? null
        : Error.Validation("Configure Payroll:WorkingDaysPerMonth to a value between 1 and 31 before generating payroll.");

    public static HRMS.Domain.Entities.Payroll Calculate(HRMS.Domain.Entities.Employee employee, int month, int year, int workingDays,
        IReadOnlyList<PayrollAttendanceDay> attendance, IReadOnlyList<PayrollLeavePeriod> unpaidLeave,
        DateTime createdAt)
    {
        var start = new DateOnly(year, month, 1);
        var endExclusive = start.AddMonths(1);
        var leaveDays = new HashSet<DateOnly>();
        foreach (var leave in unpaidLeave)
        {
            var leaveStart = leave.StartDate > start ? leave.StartDate : start;
            var monthEnd = endExclusive.AddDays(-1);
            var leaveEnd = leave.EndDate < monthEnd ? leave.EndDate : monthEnd;
            for (var day = leaveStart; day <= leaveEnd; day = day.AddDays(1)) leaveDays.Add(day);
        }
        var absentDays = attendance
            .Where(day => day.Status == AttendanceStatus.Absent && !leaveDays.Contains(day.Date))
            .Select(day => day.Date).ToHashSet().Count;
        var lateDays = attendance.Count(day => day.Status == AttendanceStatus.Late);
        var dailySalary = employee.BaseSalary / workingDays;
        var absenceDeduction = decimal.Round(absentDays * dailySalary, 2, MidpointRounding.AwayFromZero);
        var lateDeduction = lateDays * 300m;
        var unpaidLeaveDeduction = decimal.Round(leaveDays.Count * dailySalary, 2, MidpointRounding.AwayFromZero);

        return new HRMS.Domain.Entities.Payroll
        {
            EmployeeId = employee.Id,
            Month = month,
            Year = year,
            BaseSalary = employee.BaseSalary,
            AbsenceDeduction = absenceDeduction,
            LateDeduction = lateDeduction,
            UnpaidLeaveDeduction = unpaidLeaveDeduction,
            NetSalary = employee.BaseSalary - absenceDeduction - lateDeduction - unpaidLeaveDeduction,
            CreatedAt = createdAt
        };
    }
}
