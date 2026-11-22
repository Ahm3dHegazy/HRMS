namespace HRMS.Application.Features.Leave.Validators;

public sealed class RejectLeaveRequestCommandValidator : AbstractValidator<RejectLeaveRequestCommand>
{
    public RejectLeaveRequestCommandValidator()
    {
        RuleFor(command => command.RejectionReason)
            .RequiredText("A rejection reason", EntityLengthConstraints.LeaveReason);
    }
}

