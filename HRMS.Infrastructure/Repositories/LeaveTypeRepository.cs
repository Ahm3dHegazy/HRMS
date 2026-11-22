using System.Collections.Frozen;

namespace HRMS.Infrastructure.Repositories;

public sealed class LeaveTypeRepository(AppDbContext dbContext, IMemoryCache cache)
    : GenericRepository<LeaveType>(dbContext), ILeaveTypeRepository
{

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        var leaveTypeIds = await cache.GetOrCreateAsync<FrozenSet<int>>(
            ReferenceDataCacheKeys.LeaveTypes,
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var ids = await dbContext.LeaveTypes.AsNoTracking()
                    .Select(leaveType => leaveType.Id)
                    .ToListAsync(cancellationToken);
                return ids.ToFrozenSet();
            });

        return leaveTypeIds?.Contains(id) == true;
    }
}
