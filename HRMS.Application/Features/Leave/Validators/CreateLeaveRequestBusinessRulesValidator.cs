namespace HRMS.Application.Features.Leave.Validators;

public sealed class CreateLeaveRequestBusinessRulesValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestBusinessRulesValidator(IEmployeeRepository employees, ILeaveTypeRepository leaveTypes)
    {
        RuleFor(command => command.EmployeeId)
            .MustAsync(async (id, cancellationToken) => await employees.GetByIdAsync(id) is not null)
            .When(command => command.EmployeeId > 0)
            .WithMessage("The employee does not exist.");
        RuleFor(command => command.LeaveTypeId)
            .MustAsync(leaveTypes.ExistsAsync)
            .When(command => command.LeaveTypeId > 0)
            .WithMessage("The leave type does not exist.");
        RuleFor(command => command.StartDate)
            .Must((command, startDate) => startDate <= command.EndDate)
            .When(command => command.StartDate != default && command.EndDate != default)
            .WithMessage("The start date must be on or before the end date.");
        RuleFor(command => command.StartDate)
            .Must(startDate => startDate > DateOnly.FromDateTime(DateTime.Today))
            .When(command => command.StartDate != default)
            .WithMessage("The start date must be in the future.");
        RuleFor(command => command.EndDate)
            .Must(endDate => endDate > DateOnly.FromDateTime(DateTime.Today))
            .When(command => command.EndDate != default)
            .WithMessage("The end date must be in the future.");
    }
}
