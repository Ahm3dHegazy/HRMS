namespace HRMS.Application.Validators.EmployeeValidators;

public sealed class UpdateEmployeeBusinessRulesValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeBusinessRulesValidator(IDepartmentRepository departments,
        IJobTitleRepository jobTitles,
        IEmployeeRepository employees)
    {
        RuleFor(command => command.Email)
            .MustAsync(async (command, email, _) =>
                !await employees.EmailExistsAsync(email.Trim(), command.EmployeeId))
            .When(command => !string.IsNullOrWhiteSpace(command.Email) && command.EmployeeId > 0)
            .WithMessage("This email is already registered.")
            .WithErrorCode("Conflict");
        RuleFor(command => command.Phone)
            .MustAsync(async (command, phone, _) =>
                !await employees.PhoneExistsAsync(phone.Trim(), command.EmployeeId))
            .When(command => !string.IsNullOrWhiteSpace(command.Phone) && command.EmployeeId > 0)
            .WithMessage("This phone number is already registered.")
            .WithErrorCode("Conflict");
        RuleFor(command => command.DepartmentId)
            .MustAsync(async (id, cancellationToken) => await departments.GetByIdAsync(id) is not null)
            .When(command => command.DepartmentId > 0)
            .WithMessage("The selected department does not exist.");
        RuleFor(command => command.JobTitleId)
            .MustAsync(async (id, cancellationToken) => await jobTitles.GetByIdAsync(id) is not null)
            .When(command => command.JobTitleId > 0)
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
            .GreaterThan(10000).WithMessage("Base salary must be greater than 10000.");
    }
}
