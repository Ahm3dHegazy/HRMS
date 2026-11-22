using HRMS.Application.Features.AttendanceRecord.Commands.CreateAttendanceRecord;

namespace HRMS.Application.Features.AttendanceRecord.Validators;

public sealed class CreateAttendanceRecordCommandValidator : AbstractValidator<CreateAttendanceRecordCommand>
{
    public CreateAttendanceRecordCommandValidator()
    {
        RuleFor(command => command.EmployeeId).GreaterThan(0)
            .WithMessage("A valid employee ID is required.");
        RuleFor(command => command.Date).NotEqual(default(DateOnly))
            .WithMessage("Date is required.");
        RuleFor(command => command.Status).IsInEnum();
    }
}

