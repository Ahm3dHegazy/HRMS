using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HRMS.Infrastructure.Data;

public sealed class ReferenceDataCacheInvalidationInterceptor(IMemoryCache cache) : SaveChangesInterceptor
{
    private readonly HashSet<Type> _changedEntityTypes = [];

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        CaptureChangedEntityTypes(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        CaptureChangedEntityTypes(eventData.Context);
        return ValueTask.FromResult(result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        InvalidateChangedEntries();
        return result;
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        InvalidateChangedEntries();
        return ValueTask.FromResult(result);
    }

    private void CaptureChangedEntityTypes(DbContext? dbContext)
    {
        if (dbContext is null)
            return;

        foreach (var entry in dbContext.ChangeTracker.Entries()
                     .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            _changedEntityTypes.Add(entry.Metadata.ClrType);
        }
    }

    private void InvalidateChangedEntries()
    {
        if (_changedEntityTypes.Contains(typeof(Employee)) || _changedEntityTypes.Contains(typeof(Department)))
            cache.Remove(ReferenceDataCacheKeys.Departments);
        if (_changedEntityTypes.Contains(typeof(JobTitle)))
            cache.Remove(ReferenceDataCacheKeys.JobTitles);
        if (_changedEntityTypes.Contains(typeof(LeaveType)))
            cache.Remove(ReferenceDataCacheKeys.LeaveTypes);
        if (_changedEntityTypes.Contains(typeof(PublicHoliday)))
            cache.Remove(ReferenceDataCacheKeys.PublicHolidays);

        _changedEntityTypes.Clear();
    }
}
