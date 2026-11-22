namespace HRMS.Application.Common.Attendance;

public interface IAttendanceClock
{
    DateTimeOffset GetLocalNow();
}
