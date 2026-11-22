namespace HRMS.Infrastructure.Repositories;

public sealed class AttendanceRecordRepository(AppDbContext dbContext)
    : GenericRepository<AttendanceRecord>(dbContext), IAttendanceRecordRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    private IQueryable<AttendanceRecord> ReadOnlyRecords => _dbContext.AttendanceRecords.AsNoTracking();

    public async Task<IReadOnlyList<AttendanceRecordDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var query = ReadOnlyRecords
            .OrderBy(record => record.Date)
            .ThenBy(record => record.Employee.FullName)
            .ThenBy(record => record.Id);

        return await ToDtosAsync(query, cancellationToken);
    }

    public async Task<IReadOnlyList<AttendanceRecordDto>> GetByEmployeeIdAsync(int employeeId,
        CancellationToken cancellationToken)
    {
        var query = ReadOnlyRecords
            .Where(record => record.EmployeeId == employeeId)
            .OrderByDescending(record => record.Date)
            .ThenByDescending(record => record.Id);

        return await ToDtosAsync(query, cancellationToken);
    }

    public Task<IReadOnlyList<AttendanceRecordDto>> GetByEmployeeIdForPeriodAsync(int employeeId,
        DateOnly startDate, DateOnly endDateExclusive, CancellationToken cancellationToken)
    {
        var query = ReadOnlyRecords
            .Where(record => record.EmployeeId == employeeId
                             && record.Date >= startDate
                             && record.Date < endDateExclusive)
            .OrderBy(record => record.Date)
            .ThenBy(record => record.Id);

        return ToDtosAsync(query, cancellationToken);
    }

    public async Task<AttendanceRecordDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var records = await ToDtosAsync(ReadOnlyRecords.Where(record => record.Id == id), cancellationToken);
        return records.SingleOrDefault();
    }

    public Task<AttendanceRecord?> GetForEmployeeOnDateAsync(int employeeId, DateOnly date,
        CancellationToken cancellationToken)
    {
        return _dbContext.AttendanceRecords
            .SingleOrDefaultAsync(record => record.EmployeeId == employeeId && record.Date == date,
                cancellationToken);
    }

    public Task<bool> ExistsForEmployeeOnDateAsync(int employeeId, DateOnly date, int? excludingId,
        CancellationToken cancellationToken)
    {
        return _dbContext.AttendanceRecords.AnyAsync(record =>
            record.EmployeeId == employeeId
            && record.Date == date
            && (excludingId == null || record.Id != excludingId), cancellationToken);
    }

    public async Task<bool> TryCheckInAsync(int id, TimeOnly checkIn, AttendanceStatus status,
        CancellationToken cancellationToken) => await _dbContext.AttendanceRecords
        .Where(record => record.Id == id && record.CheckIn == null
            && record.Status != AttendanceStatus.Absent && record.Status != AttendanceStatus.OnLeave)
        .ExecuteUpdateAsync(setters => setters.SetProperty(record => record.CheckIn, checkIn)
            .SetProperty(record => record.Status, status), cancellationToken) == 1;

    public async Task<bool> TryCheckOutAsync(int id, TimeOnly checkOut, CancellationToken cancellationToken) =>
        await _dbContext.AttendanceRecords
            .Where(record => record.Id == id && record.CheckIn != null && record.CheckOut == null
                && record.CheckIn <= checkOut
                && (record.Status == AttendanceStatus.Present || record.Status == AttendanceStatus.Late))
            .ExecuteUpdateAsync(setters => setters.SetProperty(record => record.CheckOut, checkOut), cancellationToken) == 1;

    public async Task<bool> TryAddAttendanceRecordAsync(AttendanceRecord record, CancellationToken cancellationToken)
    {
        await _dbContext.AttendanceRecords.AddAsync(record, cancellationToken);
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (exception.GetBaseException() is Microsoft.Data.SqlClient.SqlException { Number: 2601 or 2627 })
        {
            _dbContext.Entry(record).State = EntityState.Detached;
            return false;
        }
    }

    private static async Task<IReadOnlyList<AttendanceRecordDto>> ToDtosAsync(IQueryable<AttendanceRecord> query,
        CancellationToken cancellationToken)
    {
        var rows = await query
            .Select(record => new
            {
                record.Id,
                record.EmployeeId,
                EmployeeName = record.Employee.FullName,
                record.Date,
                record.CheckIn,
                record.CheckOut,
                record.Status
            })
            .ToListAsync(cancellationToken);

        return rows.ConvertAll(row => new AttendanceRecordDto(row.Id, row.EmployeeId, row.EmployeeName,
            row.Date, row.CheckIn, row.CheckOut, row.Status.ToString()));
    }
}
