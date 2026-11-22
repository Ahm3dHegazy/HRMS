namespace HRMS.Application.Features.AttendanceRecord.Validators;

internal static class AttendanceRecordBusinessRules
{
    public static void AddCheckOutAfterCheckInRule<T>(AbstractValidator<T> validator,
        Func<T, TimeOnly?> getCheckIn, Func<T, TimeOnly?> getCheckOut)
    {
        validator.RuleFor(record => record)
            .Must(record => !getCheckOut(record).HasValue || !getCheckIn(record).HasValue ||
                            getCheckOut(record)!.Value >= getCheckIn(record)!.Value)
            .WithMessage("Check-out time must be at or after check-in time.");
    }
}
