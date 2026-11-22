namespace HRMS.Application.Common.Validation;

public static class StringFormatRules
{
    public static IRuleBuilderOptions<T, string?> RequiredText<T>(
        this IRuleBuilderInitial<T, string?> rule,
        string fieldName,
        int maxLength)
    {
        return rule.Cascade(CascadeMode.Stop)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage($"{fieldName} is required.")
            .MaximumLength(maxLength)
            .WithMessage($"{fieldName} cannot exceed {maxLength} characters.");
    }

    public static IRuleBuilderOptions<T, string?> EmailAddress<T>(
        this IRuleBuilderInitial<T, string?> rule,
        int maxLength)
    {
        return rule.Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(maxLength).WithMessage($"Email cannot exceed {maxLength} characters.");
    }

    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(
        this IRuleBuilderInitial<T, string?> rule,
        int maxLength)
    {
        return rule.Cascade(CascadeMode.Stop)
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Phone is required.")
            .MaximumLength(maxLength).WithMessage($"Phone cannot exceed {maxLength} characters.");
    }

    public static IRuleBuilderOptions<T, string?> OptionalText<T>(
        this IRuleBuilderInitial<T, string?> rule,
        string fieldName,
        int maxLength)
    {
        return rule.MaximumLength(maxLength)
            .WithMessage($"{fieldName} cannot exceed {maxLength} characters.");
    }
}
