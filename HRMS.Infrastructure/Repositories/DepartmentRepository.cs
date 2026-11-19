namespace HRMS.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext dbContext, IMemoryCache cache)
    : GenericRepository<Department>(dbContext), IDepartmentRepository
{
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

    public Task<bool> HasActiveEmployeesAsync(int departmentId, CancellationToken cancellationToken)
    {
        return dbContext.Employees.AnyAsync(employee =>
                employee.DepartmentId == departmentId && employee.Status == EmploymentStatus.Active,
            cancellationToken);
    }

    private IQueryable<DepartmentDetailsDto> DepartmentDetailsQuery()
    {
        return dbContext.Departments.AsNoTracking().Select(department => new DepartmentDetailsDto
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
