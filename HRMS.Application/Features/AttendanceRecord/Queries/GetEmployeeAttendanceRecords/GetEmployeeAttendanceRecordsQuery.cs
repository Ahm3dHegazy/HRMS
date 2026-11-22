namespace HRMS.Application.Features.AttendanceRecord.Queries.GetEmployeeAttendanceRecords;

public sealed record GetEmployeeAttendanceRecordsQuery(int EmployeeId) : IRequest<Result<IReadOnlyList<AttendanceRecordDto>>>;

