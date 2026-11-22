namespace HRMS.Application.Features.Employee.Validators;

public sealed class AddEmployeeBusinessRulesValidator : AbstractValidator<AddEmployeeCommand>
{
    public AddEmployeeBusinessRulesValidator(IDepartmentRepository departments,
        IJobTitleRepository jobTitles,
        IEmployeeRepository employees)
    {
        RuleFor(command => command.Email)
            .MustAsync(async (email, _) => !await employees.EmailExistsAsync(email.Trim()))
            .When(command => !string.IsNullOrWhiteSpace(command.Email))
            .WithMessage("This email is already registered.")
            .WithErrorCode("Conflict");
        
        RuleFor(command => command.Phone)
            .MustAsync(async (phone, _) => !await employees.PhoneExistsAsync(phone.Trim()))
            .When(command => !string.IsNullOrWhiteSpace(command.Phone))
            .WithMessage("This phone number is already registered.")
            .WithErrorCode("Conflict");
        
        RuleFor(command => command.NationalId)
            .MustAsync(async (nationalId, _) => !await employees.NationalIdExistsAsync(nationalId.Trim()))
            .When(command => !string.IsNullOrWhiteSpace(command.NationalId))
            .WithMessage("This national ID is already registered.")
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
            .MustAsync(async (id, cancellationToken) =>
                (await employees.GetByIdAsync(id))?.Status == EmploymentStatus.Active)
            .When(command => command.ManagerId > 0)
            .WithMessage("The selected manager does not exist or is not active.");

        RuleFor(command => command.BaseSalary)
            .GreaterThan(10000).WithMessage("Base salary must be greater than 10000.");
        RuleFor(command => command.HireDate)
            .Must(hireDate =>
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                return hireDate >= today && hireDate <= today.AddDays(30);
            })
            .WithMessage("Hire date must be between today and the next 30 days.");
    }
}
