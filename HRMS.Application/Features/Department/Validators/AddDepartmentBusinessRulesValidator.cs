namespace HRMS.Application.Features.Department.Validators;

public sealed class AddDepartmentBusinessRulesValidator : AbstractValidator<AddDepartmentCommand>
{
    public AddDepartmentBusinessRulesValidator(IDepartmentRepository departments, IEmployeeRepository employees)
    {
        RuleFor(command => command.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                var existing = await departments.GetAllAsync();
                return !existing.Any(department => string.Equals(
                    department.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
            })
            .When(command => !string.IsNullOrWhiteSpace(command.Name))
            .WithMessage("A department with this name already exists.")
            .WithErrorCode("Conflict");

        RuleFor(command => command.ManagerId)
            .MustAsync(async (managerId, cancellationToken) =>
            {
                if (managerId is null) return true;
                var manager = await employees.GetByIdAsync(managerId.Value);
                return manager?.Status == EmploymentStatus.Active;
            })
            .When(command => command.ManagerId.HasValue && command.ManagerId > 0)
            .WithMessage("The selected manager does not exist or is not active.");
    }
}
