namespace HRMS.Application.Features.AttendanceRecord.Commands.DeleteAttendanceRecord;

public sealed record DeleteAttendanceRecordCommand(int Id) : IRequest<Result<bool>>;

