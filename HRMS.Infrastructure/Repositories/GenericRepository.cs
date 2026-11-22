namespace HRMS.Infrastructure.Repositories;

public class GenericRepository<TEntity>(AppDbContext dbContext)
    : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entities = dbContext.Set<TEntity>();

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public Task<int> SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}
