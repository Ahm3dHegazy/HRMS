namespace HRMS.Application.Features.AttendanceRecord.Commands.UpdateAttendanceRecord;

public sealed record UpdateAttendanceRecordCommand(
    int Id,
    int EmployeeId,
    DateOnly Date,
    TimeOnly? CheckIn,
    TimeOnly? CheckOut,
    AttendanceStatus Status) : IRequest<Result<AttendanceRecordDto>>;

