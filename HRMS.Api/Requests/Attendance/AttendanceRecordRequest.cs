namespace HRMS.Api.Requests.Attendance;

public sealed class AttendanceRecordRequest
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }
    public AttendanceStatus Status { get; set; }
}
