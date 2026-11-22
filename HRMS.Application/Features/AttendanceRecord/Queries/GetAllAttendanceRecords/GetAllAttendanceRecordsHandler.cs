namespace HRMS.Application.Features.AttendanceRecord.Queries.GetAllAttendanceRecords;

public sealed class GetAllAttendanceRecordsHandler(IAttendanceRecordRepository repository)
    : IRequestHandler<GetAllAttendanceRecordsQuery, IReadOnlyList<AttendanceRecordDto>>
{
    public Task<IReadOnlyList<AttendanceRecordDto>> Handle(GetAllAttendanceRecordsQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetAllAsync(cancellationToken);
    }
}

