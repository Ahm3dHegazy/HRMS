using System.Linq.Expressions;

namespace HRMS.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext dbContext)
    : GenericRepository<Employee>(dbContext), IEmployeeRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<EmployeeDetailsDto>> GetAllWithDetailsAsync(CancellationToken cancellationToken)
    {
        return await EmployeeDetailsQuery().Select(ToEmployeeDetails()).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<EmployeeDetailsDto>> GetByJobTitleIdAsync(int jobTitleId,
        CancellationToken cancellationToken)
    {
        return await EmployeeDetailsQuery()
            .Where(employee => employee.JobTitleId == jobTitleId)
            .OrderBy(employee => employee.Id)
            .Select(ToEmployeeDetails())
            .ToListAsync(cancellationToken);
    }

    public Task<EmployeeDetailsDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken)
    {
        return EmployeeDetailsQuery()
            .Where(employee => employee.Id == id)
            .Select(ToEmployeeDetails())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<EmployeeDetailsDto>> GetEmployeesPageAsync(int pageNumber, int pageSize,
        EmployeeFilter employeeFilter, CancellationToken cancellationToken)
    {
        return await ApplyEmployeeFilter(_dbContext.Employees.AsNoTracking(), employeeFilter)
            .OrderBy(employee => employee.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ToEmployeeDetails())
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetFilteredEmployeesCountAsync(EmployeeFilter employeeFilter,
        CancellationToken cancellationToken)
    {
        return ApplyEmployeeFilter(_dbContext.Employees.AsNoTracking(), employeeFilter)
            .CountAsync(cancellationToken);
    }

    private IQueryable<Employee> EmployeeDetailsQuery() => _dbContext.Employees.AsNoTracking();

    private static Expression<Func<Employee, EmployeeDetailsDto>> ToEmployeeDetails()
    {
        return employee => new EmployeeDetailsDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Phone = employee.Phone,
            BaseSalary = employee.BaseSalary,
            StatusName = employee.Status.ToString(),
            DepartmentName = employee.Department.Name,
            JobName = employee.JobTitle.Name
        };
    }

    private static IQueryable<Employee> ApplyEmployeeFilter(IQueryable<Employee> employeesQuery,
        EmployeeFilter employeeFilter)
    {
        if (employeeFilter.DepartmentId.HasValue)
            employeesQuery = employeesQuery.Where(employee => employee.DepartmentId == employeeFilter.DepartmentId.Value);

        if (employeeFilter.Status.HasValue)
            employeesQuery = employeesQuery.Where(employee => employee.Status == employeeFilter.Status.Value);

        if (!string.IsNullOrWhiteSpace(employeeFilter.Search))
        {
            var search = employeeFilter.Search.Trim();
            employeesQuery = employeesQuery.Where(employee =>
                employee.FullName.Contains(search) || employee.Phone.Contains(search) || employee.Email.Contains(search));
        }

        return employeesQuery;
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludingEmployeeId = null)
    {
        return await _dbContext.Employees.AnyAsync(e =>
            e.Email == email && (!excludingEmployeeId.HasValue || e.Id != excludingEmployeeId.Value));
    }

    public async Task<bool> NationalIdExistsAsync(string nationalId, int? excludingEmployeeId = null)
    {
        return await _dbContext.Employees.AnyAsync(e =>
            e.NationalId == nationalId && (!excludingEmployeeId.HasValue || e.Id != excludingEmployeeId.Value));
    }

    public async Task<bool> PhoneExistsAsync(string phone, int? excludingEmployeeId = null)
    {
        return await _dbContext.Employees.AnyAsync(e =>
            e.Phone == phone && (!excludingEmployeeId.HasValue || e.Id != excludingEmployeeId.Value));
    }
}
