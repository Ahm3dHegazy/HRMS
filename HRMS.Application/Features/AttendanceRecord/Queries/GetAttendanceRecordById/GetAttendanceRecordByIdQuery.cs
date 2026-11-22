namespace HRMS.Application.Features.AttendanceRecord.Queries.GetAttendanceRecordById;

public sealed record GetAttendanceRecordByIdQuery(int Id) : IRequest<Result<AttendanceRecordDto>>;

