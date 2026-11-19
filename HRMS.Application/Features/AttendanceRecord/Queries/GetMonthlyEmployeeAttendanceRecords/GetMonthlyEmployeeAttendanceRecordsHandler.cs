namespace HRMS.Application.Features.AttendanceRecord.Queries.GetMonthlyEmployeeAttendanceRecords;

public sealed class GetMonthlyEmployeeAttendanceRecordsHandler(IAttendanceRecordRepository repository,
    IEmployeeRepository employees)
    : IRequestHandler<GetMonthlyEmployeeAttendanceRecordsQuery, Result<IReadOnlyList<AttendanceRecordDto>>>
{
    public async Task<Result<IReadOnlyList<AttendanceRecordDto>>> Handle(
        GetMonthlyEmployeeAttendanceRecordsQuery query, CancellationToken cancellationToken)
    {
        if (await employees.GetByIdAsync(query.EmployeeId) is null)
            return Result<IReadOnlyList<AttendanceRecordDto>>.Failure(
                Error.NotFound($"Employee with ID {query.EmployeeId} was not found."));

        var startDate = new DateOnly(query.Year, query.Month, 1);
        var records = await repository.GetByEmployeeIdForPeriodAsync(query.EmployeeId, startDate,
            startDate.AddMonths(1), cancellationToken);

        return Result<IReadOnlyList<AttendanceRecordDto>>.Success(records);
    }
}

