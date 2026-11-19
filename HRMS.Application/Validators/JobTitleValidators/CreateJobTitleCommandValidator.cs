namespace HRMS.Application.Validators.JobTitleValidators;

public sealed class CreateJobTitleCommandValidator : AbstractValidator<CreateJobTitleCommand>
{
    public CreateJobTitleCommandValidator()
    {
        RuleFor(command => command.Name)
            .RequiredText("Job title name", EntityLengthConstraints.OrganizationName);
        RuleFor(command => command.Description)
            .OptionalText("Job title description", EntityLengthConstraints.OrganizationDescription);
    }
}
