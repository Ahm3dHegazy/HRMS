namespace HRMS.Application.Features.AttendanceRecord.Commands.UpdateAttendanceRecord;

public sealed class UpdateAttendanceRecordHandler(IAttendanceRecordRepository repository)
    : IRequestHandler<UpdateAttendanceRecordCommand, Result<AttendanceRecordDto>>
{
    public async Task<Result<AttendanceRecordDto>> Handle(UpdateAttendanceRecordCommand command,
        CancellationToken cancellationToken)
    {
        var record = await repository.GetByIdAsync(command.Id);
        if (record is null)
            return Result<AttendanceRecordDto>.Failure(Error.NotFound(
                $"Attendance record with ID {command.Id} was not found."));

        if (await repository.ExistsForEmployeeOnDateAsync(command.EmployeeId, command.Date, command.Id,
                cancellationToken))
            return Result<AttendanceRecordDto>.Failure(Error.Conflict(
                $"An attendance record already exists for employee {command.EmployeeId} on {command.Date}."));

        var outcome = record.Update(command.EmployeeId, command.Date, command.CheckIn, command.CheckOut,
            command.Status);
        if (outcome != Domain.Entities.AttendanceRecord.UpdateOutcome.Succeeded)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Attendance update violates the attendance rules."));
        
        repository.Update(record);
        await repository.SaveChangesAsync();

        var dto = await repository.GetByIdAsync(record.Id, cancellationToken);
        return Result<AttendanceRecordDto>.Success(dto!);
    }
}

