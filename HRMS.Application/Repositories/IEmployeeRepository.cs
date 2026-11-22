namespace HRMS.Application.Repositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<IReadOnlyList<EmployeeDetailsDto>> GetAllWithDetailsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<EmployeeDetailsDto>> GetByJobTitleIdAsync(int jobTitleId, CancellationToken cancellationToken);
    Task<EmployeeDetailsDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<EmployeeDetailsDto>> GetEmployeesPageAsync(int pageNumber, int pageSize,
        EmployeeFilter employeeFilter, CancellationToken cancellationToken);
    Task<int> GetFilteredEmployeesCountAsync(EmployeeFilter employeeFilter, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, int? excludingEmployeeId = null);
    Task<bool> NationalIdExistsAsync(string nationalId, int? excludingEmployeeId = null);
    Task<bool> PhoneExistsAsync(string phone, int? excludingEmployeeId = null);
}
