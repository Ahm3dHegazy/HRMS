using HRMS.Application.Common.Attendance;
using Microsoft.Extensions.Options;

namespace HRMS.Infrastructure.Attendance;

public sealed class AttendanceClock : IAttendanceClock
{
    private readonly TimeProvider _timeProvider;
    private readonly TimeZoneInfo _timeZone;

    public AttendanceClock(TimeProvider timeProvider, IOptions<AttendanceSettings> settings)
    {
        _timeProvider = timeProvider;
        try { _timeZone = TimeZoneInfo.FindSystemTimeZoneById(settings.Value.TimeZoneId); }
        catch (TimeZoneNotFoundException exception)
        {
            throw new InvalidOperationException($"Attendance time zone '{settings.Value.TimeZoneId}' was not found.", exception);
        }
    }

    public DateTimeOffset GetLocalNow() => TimeZoneInfo.ConvertTime(_timeProvider.GetUtcNow(), _timeZone);
}
