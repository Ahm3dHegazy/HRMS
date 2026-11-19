namespace HRMS.Application.Features.AttendanceRecord.Queries.GetAttendanceRecordById;

public sealed class GetAttendanceRecordByIdHandler(IAttendanceRecordRepository repository)
    : IRequestHandler<GetAttendanceRecordByIdQuery, Result<AttendanceRecordDto>>
{
    public async Task<Result<AttendanceRecordDto>> Handle(GetAttendanceRecordByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await repository.GetByIdAsync(query.Id, cancellationToken);
        return record is null
            ? Result<AttendanceRecordDto>.Failure(Error.NotFound($"Attendance record with ID {query.Id} was not found."))
            : Result<AttendanceRecordDto>.Success(record);
    }
}

