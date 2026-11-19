namespace HRMS.Application.Dtos.Attendance;

public sealed record AttendanceRecordDto(
    int Id,
    int EmployeeId,
    string EmployeeName,
    DateOnly Date,
    TimeOnly? CheckIn,
    TimeOnly? CheckOut,
    string Status);
