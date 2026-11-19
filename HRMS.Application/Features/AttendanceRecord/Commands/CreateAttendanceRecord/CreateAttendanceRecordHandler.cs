namespace HRMS.Application.Features.AttendanceRecord.Commands.CreateAttendanceRecord;

public sealed class CreateAttendanceRecordHandler(IAttendanceRecordRepository repository,
    Microsoft.Extensions.Options.IOptions<HRMS.Application.Common.Attendance.AttendanceSettings> settings)
    : IRequestHandler<CreateAttendanceRecordCommand, Result<AttendanceRecordDto>>
{
    public async Task<Result<AttendanceRecordDto>> Handle(CreateAttendanceRecordCommand command,
        CancellationToken cancellationToken)
    {
        if (await repository.ExistsForEmployeeOnDateAsync(command.EmployeeId, command.Date, null, cancellationToken))
            return Result<AttendanceRecordDto>.Failure(Error.Conflict(
                $"An attendance record already exists for employee {command.EmployeeId} on {command.Date}."));

        var record = new Domain.Entities.AttendanceRecord();
        var outcome = record.Initialize(command.EmployeeId, command.Date, command.CheckIn, command.CheckOut,
            command.Status, settings.Value.ToPolicy());
        if (outcome != Domain.Entities.AttendanceRecord.UpdateOutcome.Succeeded)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Attendance details violate the attendance rules."));
        await repository.AddAsync(record);
        await repository.SaveChangesAsync();

        var dto = await repository.GetByIdAsync(record.Id, cancellationToken);
        return Result<AttendanceRecordDto>.Success(dto!);
    }
}

