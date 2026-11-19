using HRMS.Application.Features.AttendanceRecord.Commands.UpdateAttendanceRecord;

namespace HRMS.Application.Validators.AttendanceRecordValidators;

public sealed class UpdateAttendanceRecordBusinessRulesValidator : AbstractValidator<UpdateAttendanceRecordCommand>
{
    public UpdateAttendanceRecordBusinessRulesValidator(IEmployeeRepository employees)
    {
        RuleFor(command => command.EmployeeId)
            .MustAsync(async (id, cancellationToken) => await employees.GetByIdAsync(id) is not null)
            .When(command => command.EmployeeId > 0)
            .WithMessage("The employee does not exist.");
        AttendanceRecordBusinessRules.AddCheckOutAfterCheckInRule(this,
            command => command.CheckIn, command => command.CheckOut);
    }
}
