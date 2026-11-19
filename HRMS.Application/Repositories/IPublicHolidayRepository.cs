namespace HRMS.Application.Repositories;

public interface IPublicHolidayRepository : IGenericRepository<PublicHoliday>
{
    Task<IReadOnlyList<PublicHolidayDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<PublicHolidayDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsOnDateAsync(DateOnly date, int? excludingId, CancellationToken cancellationToken);
}
