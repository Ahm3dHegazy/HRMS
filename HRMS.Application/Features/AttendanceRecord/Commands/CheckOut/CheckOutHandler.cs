namespace HRMS.Application.Features.AttendanceRecord.Commands.CheckOut;

public sealed class CheckOutHandler(
    IEmployeeRepository employees,
    IAttendanceRecordRepository records,
    HRMS.Application.Common.Attendance.IAttendanceClock clock)
    : IRequestHandler<CheckOutCommand, Result<AttendanceRecordDto>>
{
    public async Task<Result<AttendanceRecordDto>> Handle(CheckOutCommand command, CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(command.EmployeeId);
        if (employee is null)
            return Result<AttendanceRecordDto>.Failure(Error.NotFound($"Employee with ID {command.EmployeeId} was not found."));
        if (employee.Status != EmploymentStatus.Active)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Only active employees can check out."));

        var now = clock.GetLocalNow();
        var date = DateOnly.FromDateTime(now.DateTime);
        var time = TimeOnly.FromDateTime(now.DateTime);
        var record = await records.GetForEmployeeOnDateAsync(command.EmployeeId, date, cancellationToken);
        if (record is null)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("The employee must check in before checking out."));

        var checkoutOutcome = record.CheckOutAt(time);
        if (checkoutOutcome == Domain.Entities.AttendanceRecord.CheckOutOutcome.MustCheckInFirst)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("The employee must check in before checking out."));
        if (checkoutOutcome == Domain.Entities.AttendanceRecord.CheckOutOutcome.AlreadyCheckedOut)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("The employee has already checked out today."));
        if (checkoutOutcome == Domain.Entities.AttendanceRecord.CheckOutOutcome.EarlierThanCheckIn)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Check-out time cannot be earlier than check-in time."));
        if (checkoutOutcome == Domain.Entities.AttendanceRecord.CheckOutOutcome.PunchNotAllowed)
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Check-out is not allowed for this attendance status."));

        if (!await records.TryCheckOutAsync(record.Id, time, cancellationToken))
            return Result<AttendanceRecordDto>.Failure(Error.Conflict("Attendance changed concurrently. Reload and try again."));
        
        var dto = await records.GetByIdAsync(record.Id, cancellationToken);
        return Result<AttendanceRecordDto>.Success(dto!);
    }
}

