using HRMS.Application.Features.PublicHoliday.Commands.UpdatePublicHoliday;

namespace HRMS.Application.Features.PublicHoliday.Validators;

public sealed class UpdatePublicHolidayCommandValidator : AbstractValidator<UpdatePublicHolidayCommand>
{
    public UpdatePublicHolidayCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0)
            .WithMessage("A valid public holiday ID is required.");
        RuleFor(command => command.Name)
            .RequiredText("Public holiday name", EntityLengthConstraints.PublicHolidayName);
        RuleFor(command => command.Date).NotEqual(default(DateOnly)).WithMessage("Date is required.");
    }
}

