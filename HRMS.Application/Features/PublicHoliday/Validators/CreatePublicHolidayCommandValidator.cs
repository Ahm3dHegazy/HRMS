using HRMS.Application.Features.PublicHoliday.Commands.CreatePublicHoliday;

namespace HRMS.Application.Features.PublicHoliday.Validators;

public sealed class CreatePublicHolidayCommandValidator : AbstractValidator<CreatePublicHolidayCommand>
{
    public CreatePublicHolidayCommandValidator()
    {
        RuleFor(command => command.Name)
            .RequiredText("Public holiday name", EntityLengthConstraints.PublicHolidayName);
        RuleFor(command => command.Date).NotEqual(default(DateOnly)).WithMessage("Date is required.");
    }
}

