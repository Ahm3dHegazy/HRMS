namespace HRMS.Application.Common.Payroll;

public sealed class PayrollSettings
{
    public const string SectionName = "Payroll";
    public int? WorkingDaysPerMonth { get; set; }
}
