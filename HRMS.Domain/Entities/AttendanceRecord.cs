namespace HRMS.Domain.Entities;

public sealed class AttendancePolicy(TimeOnly workStartTime, TimeOnly absenceCutoffTime, int gracePeriodMinutes)
{
    public TimeOnly WorkStartTime { get; } = workStartTime;
    public TimeOnly AbsenceCutoffTime { get; } = absenceCutoffTime;
    public int GracePeriodMinutes { get; } = gracePeriodMinutes;

    public AttendanceStatus GetStatus(TimeOnly checkIn) =>
        checkIn > AbsenceCutoffTime ? AttendanceStatus.Absent :
        checkIn > WorkStartTime.AddMinutes(GracePeriodMinutes) ? AttendanceStatus.Late : AttendanceStatus.Present;
}

public class AttendanceRecord
{
    public enum CheckInOutcome { Succeeded, AlreadyCheckedIn, EmployeeIdRequired, DateRequired, AfterAbsenceCutoff, PunchNotAllowed }
    public enum CheckOutOutcome { Succeeded, MustCheckInFirst, AlreadyCheckedOut, EarlierThanCheckIn, PunchNotAllowed }
    public enum UpdateOutcome { Succeeded, InvalidEmployeeId, InvalidDate, InvalidStatus, InvalidPunches, StatusMismatch, PunchesCannotBeRemoved }

    public int Id { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly? CheckIn { get; private set; }
    public TimeOnly? CheckOut { get; private set; }
    public AttendanceStatus Status { get; private set; }
    public int EmployeeId { get; private set; }
    public Employee Employee { get; set; } = null!;

    public static AttendanceRecord CreateSeed(int employeeId, DateOnly date, TimeOnly checkIn, TimeOnly checkOut,
        AttendanceStatus status) => new()
    {
        EmployeeId = employeeId, Date = date, CheckIn = checkIn, CheckOut = checkOut, Status = status
    };

    public CheckInOutcome BeginCheckIn(int employeeId, DateOnly date, TimeOnly time, AttendancePolicy policy)
    {
        if (employeeId <= 0) return CheckInOutcome.EmployeeIdRequired;
        if (date == default) return CheckInOutcome.DateRequired;
        var status = policy.GetStatus(time);
        if (status == AttendanceStatus.Absent) return CheckInOutcome.AfterAbsenceCutoff;
        EmployeeId = employeeId;
        Date = date;
        CheckIn = time;
        Status = status;
        return CheckInOutcome.Succeeded;
    }

    public CheckInOutcome CheckInAt(TimeOnly time, AttendancePolicy policy)
    {
        if (CheckIn.HasValue) return CheckInOutcome.AlreadyCheckedIn;
        if (Status is AttendanceStatus.Absent or AttendanceStatus.OnLeave) return CheckInOutcome.PunchNotAllowed;
        var status = policy.GetStatus(time);
        if (status == AttendanceStatus.Absent) return CheckInOutcome.AfterAbsenceCutoff;
        CheckIn = time;
        Status = status;
        return CheckInOutcome.Succeeded;
    }

    public CheckOutOutcome CheckOutAt(TimeOnly time)
    {
        if (!CheckIn.HasValue) return CheckOutOutcome.MustCheckInFirst;
        if (Status is not (AttendanceStatus.Present or AttendanceStatus.Late)) return CheckOutOutcome.PunchNotAllowed;
        if (CheckOut.HasValue) return CheckOutOutcome.AlreadyCheckedOut;
        if (time < CheckIn.Value) return CheckOutOutcome.EarlierThanCheckIn;
        CheckOut = time;
        return CheckOutOutcome.Succeeded;
    }

    public UpdateOutcome Initialize(int employeeId, DateOnly date, TimeOnly? checkIn, TimeOnly? checkOut,
        AttendanceStatus status, AttendancePolicy policy) => Apply(employeeId, date, checkIn, checkOut, status, policy, false);

    public UpdateOutcome Update(int employeeId, DateOnly date, TimeOnly? checkIn, TimeOnly? checkOut,
        AttendanceStatus status, AttendancePolicy policy) => Apply(employeeId, date, checkIn, checkOut, status, policy, true);

    private UpdateOutcome Apply(int employeeId, DateOnly date, TimeOnly? checkIn, TimeOnly? checkOut,
        AttendanceStatus status, AttendancePolicy policy, bool isUpdate)
    {
        if (employeeId <= 0) return UpdateOutcome.InvalidEmployeeId;
        if (date == default) return UpdateOutcome.InvalidDate;
        if (!Enum.IsDefined(status)) return UpdateOutcome.InvalidStatus;
        if (isUpdate && (CheckIn.HasValue && checkIn != CheckIn || CheckOut.HasValue && checkOut != CheckOut))
            return UpdateOutcome.PunchesCannotBeRemoved;
        if (checkOut.HasValue && (!checkIn.HasValue || checkOut.Value < checkIn.Value)) return UpdateOutcome.InvalidPunches;
        if ((status is AttendanceStatus.Absent or AttendanceStatus.OnLeave) && (checkIn.HasValue || checkOut.HasValue))
            return UpdateOutcome.InvalidPunches;
        if ((status is AttendanceStatus.Present or AttendanceStatus.Late) && !checkIn.HasValue) return UpdateOutcome.InvalidPunches;
        if (checkIn.HasValue && policy.GetStatus(checkIn.Value) != status) return UpdateOutcome.StatusMismatch;
        EmployeeId = employeeId; Date = date; CheckIn = checkIn; CheckOut = checkOut; Status = status;
        return UpdateOutcome.Succeeded;
    }
}
