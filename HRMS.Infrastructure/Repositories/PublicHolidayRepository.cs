namespace HRMS.Infrastructure.Repositories;

public sealed class PublicHolidayRepository(AppDbContext dbContext, IMemoryCache cache)
    : GenericRepository<PublicHoliday>(dbContext), IPublicHolidayRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<PublicHolidayDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync<IReadOnlyList<PublicHolidayDto>>(ReferenceDataCacheKeys.PublicHolidays, 
            async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _dbContext.PublicHolidays.AsNoTracking()
                .OrderBy(holiday => holiday.Date)
                .ThenBy(holiday => holiday.Name)
                .Select(holiday => new PublicHolidayDto(holiday.Id, holiday.Name, holiday.Date))
                .ToListAsync(cancellationToken);
        }) ?? [];
    }

    public Task<PublicHolidayDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _dbContext.PublicHolidays.AsNoTracking()
            .Where(holiday => holiday.Id == id)
            .Select(holiday => new PublicHolidayDto(holiday.Id, holiday.Name, holiday.Date))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ExistsOnDateAsync(DateOnly date, int? excludingId, CancellationToken cancellationToken)
    {
        return _dbContext.PublicHolidays.AnyAsync(holiday => holiday.Date == date &&
                                                            (!excludingId.HasValue || holiday.Id != excludingId.Value),
            cancellationToken);
    }
}
