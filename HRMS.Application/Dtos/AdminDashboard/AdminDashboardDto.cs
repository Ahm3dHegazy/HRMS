namespace HRMS.Application.Dtos.AdminDashboard;

public sealed record AdminDashboardDto(
    int TotalEmployees,
    int ActiveEmployees,
    int EmployeesOnLeave,
    int TerminatedEmployees,
    int TotalDepartments,
    int PendingLeaveRequests);
