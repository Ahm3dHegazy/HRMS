namespace HRMS.Application.Features.AttendanceRecord.Commands.CreateAttendanceRecord;

public sealed record CreateAttendanceRecordCommand(
    int EmployeeId,
    DateOnly Date,
    TimeOnly? CheckIn,
    TimeOnly? CheckOut,
    AttendanceStatus Status) : IRequest<Result<AttendanceRecordDto>>;

