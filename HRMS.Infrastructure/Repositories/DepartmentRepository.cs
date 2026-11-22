namespace HRMS.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext dbContext, IMemoryCache cache)
    : GenericRepository<Department>(dbContext), IDepartmentRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<DepartmentDetailsDto>> GetAllDetailsAsync(CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync<IReadOnlyList<DepartmentDetailsDto>>(ReferenceDataCacheKeys.Departments, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await DepartmentDetailsQuery()
                .ToListAsync(cancellationToken);
        }) ?? [];
    }

    public Task<DepartmentDetailsDto?> GetDetailsAsync(int id, CancellationToken cancellationToken)
    {
        return DepartmentDetailsQuery()
            .Where(department => department.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<DepartmentDetailsDto>> GetPagedDetailsAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        var departmentsQuery = DepartmentDetailsQuery();
        var totalCount = await departmentsQuery.CountAsync(cancellationToken);
        var items = await departmentsQuery
            .OrderBy(department => department.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<DepartmentDetailsDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public Task<bool> HasActiveEmployeesAsync(int departmentId, CancellationToken cancellationToken)
    {
        return _dbContext.Employees.AnyAsync(employee =>
                employee.DepartmentId == departmentId && employee.Status == EmploymentStatus.Active,
            cancellationToken);
    }

    private IQueryable<DepartmentDetailsDto> DepartmentDetailsQuery()
    {
        return _dbContext.Departments.AsNoTracking().Select(department => new DepartmentDetailsDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            IsActive = department.IsActive,
            ManagerName = department.Manager == null ? null : department.Manager.FullName,
            EmployeesCount = department.Employees.Count
        });
    }
}
