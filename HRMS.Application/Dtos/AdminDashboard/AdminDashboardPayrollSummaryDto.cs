namespace HRMS.Application.Dtos.AdminDashboard;

/// <summary>
/// Payroll totals for one month. <see cref="PayrollCount"/> is 0 when payroll
/// has not been generated for that month, and every amount is then 0.
/// </summary>
public sealed record AdminDashboardPayrollSummaryDto(
    int Month,
    int Year,
    int PayrollCount,
    int PaidCount,
    int UnpaidCount,
    decimal TotalBaseSalary,
    decimal TotalAbsenceDeduction,
    decimal TotalLateDeduction,
    decimal TotalUnpaidLeaveDeduction,
    decimal TotalNetSalary,
    decimal PaidNetSalary,
    decimal UnpaidNetSalary);
