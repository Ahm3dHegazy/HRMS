namespace HRMS.Infrastructure.Repositories;

public sealed class JobTitleRepository(AppDbContext dbContext, IMemoryCache cache)
    : GenericRepository<JobTitle>(dbContext), IJobTitleRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<JobTitleDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync<IReadOnlyList<JobTitleDto>>(ReferenceDataCacheKeys.JobTitles, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _dbContext.JobTitles.AsNoTracking()
                .OrderBy(jobTitle => jobTitle.Name)
                .Select(jobTitle => new JobTitleDto(jobTitle.Id, jobTitle.Name, jobTitle.Description))
                .ToListAsync(cancellationToken);
        }) ?? [];
    }

    public Task<JobTitleDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _dbContext.JobTitles.AsNoTracking()
            .Where(jobTitle => jobTitle.Id == id)
            .Select(jobTitle => new JobTitleDto(jobTitle.Id, jobTitle.Name, jobTitle.Description))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<bool> HasEmployeesNotTerminatedAsync(int jobTitleId, CancellationToken cancellationToken)
    {
        return _dbContext.Employees.AnyAsync(employee =>
                employee.JobTitleId == jobTitleId && employee.Status != EmploymentStatus.Terminated,
            cancellationToken);
    }
}
