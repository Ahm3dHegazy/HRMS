namespace HRMS.Application.Features.JobTitle.Validators;

public sealed class UpdateJobTitleCommandValidator : AbstractValidator<UpdateJobTitleCommand>
{
    public UpdateJobTitleCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0)
            .WithMessage("A valid job title ID is required.");
        RuleFor(command => command.Name)
            .RequiredText("Job title name", EntityLengthConstraints.OrganizationName);
        RuleFor(command => command.Description)
            .OptionalText("Job title description", EntityLengthConstraints.OrganizationDescription);
    }
}
