namespace HRMS.Application.Validators.LeaveValidators;

public sealed class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator()
    {
        RuleFor(command => command.EmployeeId).GreaterThan(0)
            .WithMessage("A valid employee ID is required.");
        RuleFor(command => command.LeaveTypeId).GreaterThan(0)
            .WithMessage("A valid leave type ID is required.");
        RuleFor(command => command.StartDate).NotEqual(default(DateOnly))
            .WithMessage("A start date is required.");
        RuleFor(command => command.EndDate).NotEqual(default(DateOnly))
            .WithMessage("An end date is required.");
        RuleFor(command => command.Reason)
            .RequiredText("A reason", EntityLengthConstraints.LeaveReason);
    }
}
