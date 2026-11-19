namespace HRMS.Application.Features.AttendanceRecord.Queries.GetEmployeeAttendanceRecords;

public sealed class GetEmployeeAttendanceRecordsHandler(IAttendanceRecordRepository repository,
    IEmployeeRepository employees)
    : IRequestHandler<GetEmployeeAttendanceRecordsQuery, Result<IReadOnlyList<AttendanceRecordDto>>>
{
    public async Task<Result<IReadOnlyList<AttendanceRecordDto>>> Handle(GetEmployeeAttendanceRecordsQuery query,
        CancellationToken cancellationToken)
    {
        if (await employees.GetByIdAsync(query.EmployeeId) is null)
            return Result<IReadOnlyList<AttendanceRecordDto>>.Failure(
                Error.NotFound($"Employee with ID {query.EmployeeId} was not found."));

        return Result<IReadOnlyList<AttendanceRecordDto>>.Success(
            await repository.GetByEmployeeIdAsync(query.EmployeeId, cancellationToken));
    }
}

