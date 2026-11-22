namespace HRMS.Application.Dtos.Leave;

public sealed record LeaveBalanceDto(
    int LeaveTypeId,
    string LeaveTypeName,
    int Year,
    int TotalDays,
    int UsedDays,
    int AvailableDays);
