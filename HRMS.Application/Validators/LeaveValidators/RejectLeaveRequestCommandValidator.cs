namespace HRMS.Application.Validators.LeaveValidators;

public sealed class RejectLeaveRequestCommandValidator : AbstractValidator<RejectLeaveRequestCommand>
{
    public RejectLeaveRequestCommandValidator()
    {
        RuleFor(command => command.RejectionReason)
            .RequiredText("A rejection reason", EntityLengthConstraints.LeaveReason);
    }
}

