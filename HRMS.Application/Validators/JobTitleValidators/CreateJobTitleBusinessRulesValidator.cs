namespace HRMS.Application.Validators.JobTitleValidators;

public sealed class CreateJobTitleBusinessRulesValidator : AbstractValidator<CreateJobTitleCommand>
{
    public CreateJobTitleBusinessRulesValidator(IJobTitleRepository jobTitles)
    {
        RuleFor(command => command.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                var existing = await jobTitles.GetAllAsync();
                return !existing.Any(jobTitle => string.Equals(
                    jobTitle.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
            })
            .When(command => !string.IsNullOrWhiteSpace(command.Name))
            .WithMessage("A job title with this name already exists.")
            .WithErrorCode("Conflict");
    }
}
