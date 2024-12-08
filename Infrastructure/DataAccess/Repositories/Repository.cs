using System.Linq.Expressions;
using Application.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private const bool AutoSave = true;
    private IMyContext _dbContext;
    private DbSet<TEntity> _dbSet; // { get { return _dataBaseMainContext.Set<T>(); } }

    public Repository(IMyContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<TEntity>();
    }


    public IQueryable<TEntity> DeferredSelectAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<TEntity> FirstOrDefaultItemAsync(Expression<Func<TEntity, bool>> condition)
    {
        return await _dbSet.FirstOrDefaultAsync(condition);
    }


    public IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition)
    {
        return _dbSet.Where(condition);
    }

    public IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition, int page, int pageSize)
    {
        return DeferredWhere(condition).DeferredPaginate(page, pageSize);
    }

    public IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition, string orderByProperties)
    {
        return DeferredWhere(condition).ApplyAllOrderBy(orderByProperties);
    }

    public IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition, string orderByProperties,
        int page, int pageSize)
    {
        return DeferredWhere(condition).ApplyAllOrderBy(orderByProperties).DeferredPaginate(page, pageSize);
    }

    public IQueryable<DEntity> DeferredWhere<DEntity>(Expression<Func<DEntity, bool>> condition) where DEntity : TEntity
    {
        return _dbSet.OfType<DEntity>().Where(condition);
    }

    public IQueryable<TEntity> DeferredWhereNoTracking(Expression<Func<TEntity, bool>> condition)
    {
        return _dbSet.AsNoTracking().Where(condition);
    }

    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Any(predicate);
    }

    public TEntity Find(params object[] keyValue)
    {
        return _dbSet.Find(keyValue);
    }


    public TEntity Add(TEntity entity)
    {
        _dbSet.Add(entity);
        return SaveChanges(entity, AutoSave);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = AutoSave)
    {
        _dbSet.Update(entity);
        return await SaveChangesAsync(entity, autoSave);
    }


    public async Task<IEnumerable<TEntity>> AddAllAsync(IEnumerable<TEntity> entities, bool autoSave = AutoSave)
    {
        await _dbSet.AddRangeAsync(entities);
        return SaveChanges(entities, autoSave);
    }

    public Task<IEnumerable<TEntity>> UpdateAllAsync(IEnumerable<TEntity> entities, bool autoSave = true)
    {
        _dbSet.UpdateRange(entities);
        return Task.FromResult(SaveChanges(entities, autoSave));
    }

    public async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        if (predicate != null)
        {
            return await _dbSet.LastOrDefaultAsync(predicate);
        }

        var result = _dbSet.LastOrDefaultAsync().Result;
        return result;
    }

    public async Task<TEntity> RemoveAsync(TEntity entity, bool autoSave = AutoSave)
    {
        _dbSet.Remove(entity);
        return await SaveChangesAsync(entity, autoSave);
    }

    public async Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, bool autoSave = AutoSave)
    {
        _dbSet.RemoveRange(entities);
        return await SaveChangesAsync(autoSave);
    }

    public async Task<TEntity> AddAsync(TEntity entity, bool autoSave = AutoSave)
    {
        await _dbSet.AddAsync(entity);
        return await SaveChangesAsync(entity, autoSave);
    }

    //private TEntity SaveChanges(TEntity entity, bool autoSave)
    //{
    //    if (SaveChanges(autoSave))
    //        return entity;
    //    return null;
    //}

    //private IEnumerable<TEntity> SaveChanges(IEnumerable<TEntity> entities, bool autoSave)
    //{
    //    if (SaveChanges(autoSave))
    //        return entities;
    //    return null;
    //}

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IQueryable<TEntity> GetQueryable(bool applyIsRemovedFilter = true)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (applyIsRemovedFilter) query = query.Where(e => EF.Property<bool>(e, "IsRemoved") == false);

        return query;
    }

    private TEntity SaveChanges(TEntity entity, bool autoSave)
    {
        return SaveChanges(autoSave) ? entity : null;
    }

    private async Task<TEntity> SaveChangesAsync(TEntity entity, bool autoSave)
    {
        return await SaveChangesAsync(autoSave) ? entity : null;
    }

    private IEnumerable<TEntity> SaveChanges(IEnumerable<TEntity> entities, bool autoSave)
    {
        return SaveChanges(autoSave) ? entities : null;
    }

    public bool SaveChanges()
    {
        var succeed = _dbContext.SaveChanges();
        return succeed > 0;
    }

    public Task<bool> SaveChangesAsync()
    {
        var succeed = _dbContext.SaveChangesAsync().Result;
        return Task.FromResult(succeed > 0);
    }

    private bool SaveChanges(bool autoSave)
    {
        if (autoSave)
            return SaveChanges();
        return true;
    }

    private async Task<bool> SaveChangesAsync(bool autoSave)
    {
        if (autoSave)
            return await SaveChangesAsync();
        return true;
    }

    protected virtual void Dispose(bool disposing)
    {
        //if (_disposed) return;
        if (disposing)
            // Free other state (managed objects).
            _dbContext?.Dispose();
        //// Free your own state (unmanaged objects).
        //// Set large fields to null .
        //_utility = null;
        _dbSet = null;
        _dbContext = null;
        //_disposed = true;
    }
}