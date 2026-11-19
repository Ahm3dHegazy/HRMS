namespace HRMS.Application.Repositories;

public interface IAttendanceRecordRepository : IGenericRepository<AttendanceRecord>
{
    Task<IReadOnlyList<AttendanceRecordDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AttendanceRecordDto>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<AttendanceRecordDto>> GetByEmployeeIdForPeriodAsync(int employeeId, DateOnly startDate,
        DateOnly endDateExclusive, CancellationToken cancellationToken);
    Task<AttendanceRecordDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<AttendanceRecord?> GetForEmployeeOnDateAsync(int employeeId, DateOnly date, CancellationToken cancellationToken);
    Task<bool> ExistsForEmployeeOnDateAsync(int employeeId, DateOnly date, int? excludingId, CancellationToken cancellationToken);
    Task<bool> TryCheckInAsync(int id, TimeOnly checkIn, AttendanceStatus status, CancellationToken cancellationToken);
    Task<bool> TryCheckOutAsync(int id, TimeOnly checkOut, CancellationToken cancellationToken);
    Task<bool> TryAddAttendanceRecordAsync(AttendanceRecord record, CancellationToken cancellationToken);
}
