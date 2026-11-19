namespace HRMS.Application.Features.AttendanceRecord.Commands.CheckIn;

public sealed class CheckInHandler(IAttendanceRecordRepository records, IEmployeeRepository employees,
    HRMS.Application.Common.Attendance.IAttendanceClock clock,
    Microsoft.Extensions.Options.IOptions<HRMS.Application.Common.Attendance.AttendanceSettings> settings)
    : IRequestHandler<CheckInCommand, Result<AttendanceRecordDto>>
{
    public async Task<Result<AttendanceRecordDto>> Handle(CheckInCommand command, CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(command.EmployeeId);
        if (employee is null)
            return Result<AttendanceRecordDto>.Failure(Error.NotFound($"Employee with ID {command.EmployeeId} was not found."));
        if (employee.Status != EmploymentStatus.Active)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Only active employees can check in."));

        var now = clock.GetLocalNow();
        var date = DateOnly.FromDateTime(now.DateTime);
        var time = TimeOnly.FromDateTime(now.DateTime);
        var policy = settings.Value.ToPolicy();

        var record = await records.GetForEmployeeOnDateAsync(command.EmployeeId, date, cancellationToken);
        var isNewRecord = record is null;

        if (record is null)
        {
            record = new Domain.Entities.AttendanceRecord();
            var beginOutcome = record.BeginCheckIn(command.EmployeeId, date, time, policy);
            if (beginOutcome == Domain.Entities.AttendanceRecord.CheckInOutcome.AfterAbsenceCutoff)
                return Result<AttendanceRecordDto>.Failure(Error.Conflict("Check-in is not allowed after the absence cutoff time."));
        }

        var outcome = isNewRecord ? Domain.Entities.AttendanceRecord.CheckInOutcome.Succeeded : record.CheckInAt(time, policy);
        if (outcome == Domain.Entities.AttendanceRecord.CheckInOutcome.AlreadyCheckedIn)
        {
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("The employee has already checked in today."));
        }
        if (outcome == Domain.Entities.AttendanceRecord.CheckInOutcome.AfterAbsenceCutoff)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Check-in is not allowed after the absence cutoff time."));
        if (outcome == Domain.Entities.AttendanceRecord.CheckInOutcome.PunchNotAllowed)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Check-in is not allowed for this attendance status."));

        if (isNewRecord)
        {
            if (!await records.TryAddAttendanceRecordAsync(record, cancellationToken))
                return Result<AttendanceRecordDto>.Failure(Error.Conflict("An attendance record already exists for this employee today."));
        }
        else if (!await records.TryCheckInAsync(record.Id, time, record.Status, cancellationToken))
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Attendance changed concurrently. Reload and try again."));
        
        var dto = await records.GetByIdAsync(record.Id, cancellationToken);
        return Result<AttendanceRecordDto>.Success(dto!);
    }

}

