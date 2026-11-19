namespace HRMS.Application.Repositories;

public interface IJobTitleRepository : IGenericRepository<JobTitle>
{
    Task<IReadOnlyList<JobTitleDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<JobTitleDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasEmployeesNotTerminatedAsync(int jobTitleId, CancellationToken cancellationToken);
}
