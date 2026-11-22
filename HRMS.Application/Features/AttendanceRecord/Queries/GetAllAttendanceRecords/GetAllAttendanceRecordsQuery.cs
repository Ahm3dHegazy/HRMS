namespace HRMS.Application.Features.AttendanceRecord.Queries.GetAllAttendanceRecords;

public sealed record GetAllAttendanceRecordsQuery : IRequest<IReadOnlyList<AttendanceRecordDto>>;

