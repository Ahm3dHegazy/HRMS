using HRMS.Application.Authentication;
using HRMS.Application.Common.Attendance;
using AttendanceRecordEntity = HRMS.Domain.Entities.AttendanceRecord;
using CheckInOutcome = HRMS.Domain.Entities.AttendanceRecord.CheckInOutcome;

namespace HRMS.Application.Features.AttendanceRecord.Commands.CheckIn;

public sealed class CheckInHandler(IAttendanceRecordRepository records, IEmployeeRepository employees,
    IAttendanceClock clock, ICurrentUser currentUser)
    : IRequestHandler<CheckInCommand, Result<AttendanceRecordDto>>
{
    public async Task<Result<AttendanceRecordDto>> Handle(CheckInCommand command, CancellationToken cancellationToken)
    {
        if (currentUser.EmployeeId is not { } employeeId)
            return Failure(AttendanceErrors.AccountNotLinkedToEmployee);

        var employee = await employees.GetByIdAsync(employeeId);
        if (employee is null)
            return Failure(AttendanceErrors.EmployeeNotFound(employeeId));

        if (employee.Status != EmploymentStatus.Active)
            return Failure(AttendanceErrors.EmployeeNotActiveForCheckIn);

        var now = clock.GetLocalNow();

        var date = DateOnly.FromDateTime(now.DateTime);
        var time = TimeOnly.FromDateTime(now.DateTime);

        var existing = await records.GetForEmployeeOnDateAsync(employeeId, date, cancellationToken);

        return existing is null
            ? await CreateRecordAsync(employeeId, date, time, cancellationToken)
            : await CheckInExistingAsync(existing, time, cancellationToken);
    }

    // No record yet for today: create one and insert it.
    private async Task<Result<AttendanceRecordDto>> CreateRecordAsync(
        int employeeId, DateOnly date, TimeOnly time, CancellationToken cancellationToken)
    {
        var record = new AttendanceRecordEntity();

        var error = ToError(record.BeginCheckIn(employeeId, date, time));
        if (error is not null)
            return Failure(error);

        if (!await records.TryAddAttendanceRecordAsync(record, cancellationToken))
            return Failure(AttendanceErrors.RecordAlreadyExists);

        return await LoadAsync(record.Id, cancellationToken);
    }

    // A record already exists (e.g. pre-created as Absent/OnLeave): try to apply the check-in to it.
    private async Task<Result<AttendanceRecordDto>> CheckInExistingAsync(
        AttendanceRecordEntity record, TimeOnly time, CancellationToken cancellationToken)
    {
        var error = ToError(record.CheckInAt(time));
        if (error is not null)
            return Failure(error);

        if (!await records.TryCheckInAsync(record.Id, time, record.Status, cancellationToken))
            return Failure(AttendanceErrors.ConcurrentChange);

        return await LoadAsync(record.Id, cancellationToken);
    }

    private async Task<Result<AttendanceRecordDto>> LoadAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await records.GetByIdAsync(id, cancellationToken);
        return Result<AttendanceRecordDto>.Success(dto!);
    }

    // Single place that maps every domain outcome to an application error.
    private static Error? ToError(CheckInOutcome outcome) => outcome switch
    {
        CheckInOutcome.Succeeded => null,
        CheckInOutcome.AlreadyCheckedIn => AttendanceErrors.AlreadyCheckedIn,
        CheckInOutcome.AfterAbsenceCutoff => AttendanceErrors.AfterAbsenceCutoff,
        CheckInOutcome.PunchNotAllowed => AttendanceErrors.CheckInNotAllowedForStatus,
        CheckInOutcome.EmployeeIdRequired => AttendanceErrors.EmployeeIdRequired,
        CheckInOutcome.DateRequired => AttendanceErrors.DateRequired,
        _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, "Unhandled check-in outcome.")
    };

    private static Result<AttendanceRecordDto> Failure(Error error) =>
        Result<AttendanceRecordDto>.Failure(error);
}

