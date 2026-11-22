namespace HRMS.Application.Dtos.AdminDashboard;

public sealed record AdminDashboardLeaveSummaryDto(
    int Year,
    int TotalRequests,
    int Pending,
    int Approved,
    int Rejected,
    int Cancelled,
    IReadOnlyList<AdminDashboardLeaveTypeSummaryDto> ByLeaveType);

public sealed record AdminDashboardLeaveTypeSummaryDto(
    int LeaveTypeId,
    string LeaveTypeName,
    int Total,
    int Pending,
    int Approved,
    int Rejected,
    int Cancelled);
