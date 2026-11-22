namespace HRMS.Application.Features.AttendanceRecord.Commands.DeleteAttendanceRecord;

public sealed class DeleteAttendanceRecordHandler(IAttendanceRecordRepository repository)
    : IRequestHandler<DeleteAttendanceRecordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteAttendanceRecordCommand command, CancellationToken cancellationToken)
    {
        var record = await repository.GetByIdAsync(command.Id);
        if (record is null)
            return Result<bool>.Failure(Error.NotFound($"Attendance record with ID {command.Id} was not found."));

        repository.Delete(record);
        await repository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }
}

