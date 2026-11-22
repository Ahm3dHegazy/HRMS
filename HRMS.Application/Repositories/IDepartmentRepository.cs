namespace HRMS.Application.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<IReadOnlyList<DepartmentDetailsDto>> GetAllDetailsAsync(CancellationToken cancellationToken);
    Task<PagedList<DepartmentDetailsDto>> GetPagedDetailsAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken);
    Task<DepartmentDetailsDto?> GetDetailsAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasActiveEmployeesAsync(int departmentId, CancellationToken cancellationToken);
}
