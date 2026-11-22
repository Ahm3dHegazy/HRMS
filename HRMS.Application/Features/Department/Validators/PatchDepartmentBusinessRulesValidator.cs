using HRMS.Application.Features.Department.Commands.PatchDepartment;

namespace HRMS.Application.Features.Department.Validators;

public sealed class PatchDepartmentBusinessRulesValidator : AbstractValidator<PatchDepartmentCommand>
{
    public PatchDepartmentBusinessRulesValidator(IDepartmentRepository departments, IEmployeeRepository employees)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellationToken) => await departments.GetByIdAsync(id) is not null)
            .When(command => command.Id > 0)
            .WithMessage("The selected department does not exist.");

        RuleFor(command => command.Name)
            .MustAsync(async (command, name, cancellationToken) =>
            {
                var existing = await departments.GetAllAsync();
                return !existing.Any(department => department.Id != command.Id && string.Equals(
                    department.Name.Trim(), name!.Trim(), StringComparison.OrdinalIgnoreCase));
            })
            .When(command => !string.IsNullOrWhiteSpace(command.Name) && command.Id > 0)
            .WithMessage("A department with this name already exists.")
            .WithErrorCode("Conflict");

        RuleFor(command => command.ManagerId)
            .MustAsync(async (managerId, cancellationToken) =>
            {
                var manager = await employees.GetByIdAsync(managerId!.Value);
                return manager?.Status == EmploymentStatus.Active;
            })
            .When(command => command.ManagerId.HasValue && command.ManagerId > 0)
            .WithMessage("The selected manager does not exist or is not active.");
    }
}
