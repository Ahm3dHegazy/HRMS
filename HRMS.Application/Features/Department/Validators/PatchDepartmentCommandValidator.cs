using HRMS.Application.Features.Department.Commands.PatchDepartment;

namespace HRMS.Application.Features.Department.Validators;

public sealed class PatchDepartmentCommandValidator : AbstractValidator<PatchDepartmentCommand>
{
    public PatchDepartmentCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0)
            .WithMessage("A valid department ID is required.");
        RuleFor(command => command)
            .Must(command => command.Name is not null || command.Description is not null || command.ManagerId.HasValue)
            .WithMessage("At least one department field must be provided.");
        RuleFor(command => command.Name)
            .RequiredText("Department name", EntityLengthConstraints.OrganizationName)
            .When(command => command.Name is not null);
        RuleFor(command => command.Description)
            .OptionalText("Department description", EntityLengthConstraints.OrganizationDescription)
            .When(command => command.Description is not null);
        RuleFor(command => command.ManagerId)
            .GreaterThan(0).When(command => command.ManagerId.HasValue)
            .WithMessage("A valid manager ID is required.");
    }
}
