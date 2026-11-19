namespace HRMS.Application.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<IReadOnlyList<DepartmentDetailsDto>> GetAllDetailsAsync(CancellationToken cancellationToken);
    Task<DepartmentDetailsDto?> GetDetailsAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasActiveEmployeesAsync(int departmentId, CancellationToken cancellationToken);
}
