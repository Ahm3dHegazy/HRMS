using HRMS.Domain.Entities;

namespace HRMS.Application.Common.Attendance;

public sealed class AttendanceSettings
{
    public const string SectionName = "Attendance";
    public string WorkStartTime { get; set; } = "08:30";
    public string AbsenceCutoffTime { get; set; } = "12:00";
    public int GracePeriodMinutes { get; set; } = 15;
    public string TimeZoneId { get; set; } = "Egypt Standard Time";

    public AttendancePolicy ToPolicy()
    {
        if (!TimeOnly.TryParseExact(WorkStartTime, "HH:mm", out var start) ||
            !TimeOnly.TryParseExact(AbsenceCutoffTime, "HH:mm", out var cutoff) ||
            GracePeriodMinutes < 0 || GracePeriodMinutes > 180 || start >= cutoff)
            throw new InvalidOperationException("Attendance settings are invalid. Use HH:mm times, a valid grace period, and a cutoff after start time.");
        return new AttendancePolicy(start, cutoff, GracePeriodMinutes);
    }
}
