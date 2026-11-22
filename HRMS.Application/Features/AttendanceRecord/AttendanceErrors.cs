using HRMS.Application.Common.Results;

namespace HRMS.Application.Features.AttendanceRecord;

internal static class AttendanceErrors
{
    public static Error EmployeeNotFound(int employeeId) =>
        Error.NotFound($"Employee with ID {employeeId} was not found.");

    public static readonly Error EmployeeNotActiveForCheckIn =
        Error.Conflict("Only active employees can check in.");

    public static readonly Error AccountNotLinkedToEmployee =
        Error.Forbidden("Your account is not linked to an employee record.");

    public static readonly Error AlreadyCheckedIn =
        Error.Conflict("The employee has already checked in today.");

    public static readonly Error AfterAbsenceCutoff =
        Error.Conflict("Check-in is not allowed after the absence cutoff time.");

    public static readonly Error CheckInNotAllowedForStatus =
        Error.Conflict("Check-in is not allowed for this attendance status.");

    public static readonly Error RecordAlreadyExists =
        Error.Conflict("An attendance record already exists for this employee today.");

    public static readonly Error ConcurrentChange =
        Error.Conflict("Attendance changed concurrently. Reload and try again.");

    public static readonly Error EmployeeIdRequired =
        Error.Validation("A valid employee ID is required.");

    public static readonly Error DateRequired =
        Error.Validation("A valid attendance date is required.");
}
