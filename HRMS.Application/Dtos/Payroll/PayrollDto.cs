namespace HRMS.Application.Dtos.Payroll;

public sealed record PayrollDto(int Id, int EmployeeId, string EmployeeName, int Month, int Year,
    decimal BaseSalary, decimal AbsenceDeduction, decimal LateDeduction, decimal UnpaidLeaveDeduction,
    decimal NetSalary, string Status, DateTime CreatedAt, DateTime? PaidAt);

public sealed record PayrollAttendanceDay(DateOnly Date, AttendanceStatus Status);
public sealed record PayrollLeavePeriod(DateOnly StartDate, DateOnly EndDate);
