namespace HRMS.Application.Features.JobTitle.Validators;

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
