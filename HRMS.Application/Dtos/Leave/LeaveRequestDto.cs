namespace HRMS.Application.Dtos.Leave;

public record LeaveRequestDto(
    int Id,
    int EmployeeId,
    string LeaveTypeName,
    DateOnly StartDate,
    DateOnly EndDate,
    string Reason,
    string Status,
    DateTime? ReviewedAt,
    string? RejectionReason);
