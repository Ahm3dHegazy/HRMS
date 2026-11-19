namespace HRMS.Application.Features.AttendanceRecord.Queries.GetMonthlyEmployeeAttendanceRecords;

public sealed record GetMonthlyEmployeeAttendanceRecordsQuery(int EmployeeId, int Year, int Month)
    : IRequest<Result<IReadOnlyList<AttendanceRecordDto>>>;

