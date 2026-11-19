namespace HRMS.Application.Validators.JobTitleValidators;

public sealed class UpdateJobTitleBusinessRulesValidator : AbstractValidator<UpdateJobTitleCommand>
{
    public UpdateJobTitleBusinessRulesValidator(IJobTitleRepository jobTitles)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellationToken) => await jobTitles.GetByIdAsync(id) is not null)
            .When(command => command.Id > 0)
            .WithMessage("Job title not found.");

        RuleFor(command => command.Name)
            .MustAsync(async (command, name, cancellationToken) =>
            {
                var existing = await jobTitles.GetAllAsync();
                return !existing.Any(jobTitle => jobTitle.Id != command.Id && string.Equals(
                    jobTitle.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
            })
            .When(command => !string.IsNullOrWhiteSpace(command.Name) && command.Id > 0)
            .WithMessage("A job title with this name already exists.")
            .WithErrorCode("Conflict");
    }
}
