namespace HRMS.Application.Validators.EmployeeValidators;

public sealed class PatchEmployeeBusinessRulesValidator : AbstractValidator<PatchEmployeeCommand>
{
    public PatchEmployeeBusinessRulesValidator(IDepartmentRepository departments,
        IJobTitleRepository jobTitles,
        IEmployeeRepository employees)
    {
        RuleFor(command => command.Email)
            .MustAsync(async (command, email, _) =>
                !await employees.EmailExistsAsync(email!.Trim(), command.EmployeeId))
            .When(command => !string.IsNullOrWhiteSpace(command.Email) && command.EmployeeId > 0)
            .WithMessage("This email is already registered.")
            .WithErrorCode("Conflict");
        RuleFor(command => command.Phone)
            .MustAsync(async (command, phone, _) =>
                !await employees.PhoneExistsAsync(phone!.Trim(), command.EmployeeId))
            .When(command => !string.IsNullOrWhiteSpace(command.Phone) && command.EmployeeId > 0)
            .WithMessage("This phone number is already registered.")
            .WithErrorCode("Conflict");
        RuleFor(command => command.DepartmentId)
            .MustAsync(async (id, cancellationToken) =>
                id is null || await departments.GetByIdAsync(id.Value) is not null)
            .When(command => command.DepartmentId.HasValue)
            .WithMessage("The selected department does not exist.");
        RuleFor(command => command.JobTitleId)
            .MustAsync(async (id, cancellationToken) =>
                id is null || await jobTitles.GetByIdAsync(id.Value) is not null)
            .When(command => command.JobTitleId.HasValue)
            .WithMessage("The selected job title does not exist.");
        RuleFor(command => command.ManagerId)
            .Must((command, id) => id is null || id != command.EmployeeId)
            .When(command => command.ManagerId.HasValue && command.EmployeeId > 0)
            .WithMessage("An employee cannot be their own manager.")
            .MustAsync(async (id, cancellationToken) =>
            {
                if (id is null) return true;
                var manager = await employees.GetByIdAsync(id.Value);
                return manager?.Status == EmploymentStatus.Active;
            })
            .When(command => command.ManagerId.HasValue)
            .WithMessage("The selected manager does not exist or is not active.");
        RuleFor(command => command.BaseSalary)
            .GreaterThan(10000).When(command => command.BaseSalary.HasValue)
            .WithMessage("Base salary must be greater than 10000.");
    }
}
