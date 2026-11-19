using HRMS.Application.Features.AttendanceRecord.Commands.UpdateAttendanceRecord;

namespace HRMS.Application.Validators.AttendanceRecordValidators;

public sealed class UpdateAttendanceRecordCommandValidator : AbstractValidator<UpdateAttendanceRecordCommand>
{
    public UpdateAttendanceRecordCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0)
            .WithMessage("A valid attendance record ID is required.");
        RuleFor(command => command.EmployeeId).GreaterThan(0)
            .WithMessage("A valid employee ID is required.");
        RuleFor(command => command.Date).NotEqual(default(DateOnly))
            .WithMessage("Date is required.");
        RuleFor(command => command.Status).IsInEnum();
    }
}

