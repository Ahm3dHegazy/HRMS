using HRMS.Application.Features.AttendanceRecord.Commands.CreateAttendanceRecord;

namespace HRMS.Application.Features.AttendanceRecord.Validators;

public sealed class CreateAttendanceRecordBusinessRulesValidator : AbstractValidator<CreateAttendanceRecordCommand>
{
    public CreateAttendanceRecordBusinessRulesValidator(IEmployeeRepository employees)
    {
        RuleFor(command => command.EmployeeId)
            .MustAsync(async (id, cancellationToken) => await employees.GetByIdAsync(id) is not null)
            .When(command => command.EmployeeId > 0)
            .WithMessage("The employee does not exist.");
        AttendanceRecordBusinessRules.AddCheckOutAfterCheckInRule(this,
            command => command.CheckIn, command => command.CheckOut);
    }
}
