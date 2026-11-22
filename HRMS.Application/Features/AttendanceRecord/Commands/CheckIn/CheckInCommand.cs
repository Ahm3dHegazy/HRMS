namespace HRMS.Application.Features.AttendanceRecord.Commands.CheckIn;

public sealed record CheckInCommand : IRequest<Result<AttendanceRecordDto>>;

