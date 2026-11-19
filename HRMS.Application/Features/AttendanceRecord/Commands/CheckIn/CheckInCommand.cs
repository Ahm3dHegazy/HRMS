namespace HRMS.Application.Features.AttendanceRecord.Commands.CheckIn;

public sealed record CheckInCommand(int EmployeeId) : IRequest<Result<AttendanceRecordDto>>;

