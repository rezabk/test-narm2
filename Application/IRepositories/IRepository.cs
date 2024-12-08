using System.Linq.Expressions;

namespace Application.IRepositories;

public interface IRepository<TEntity> : IDisposable
    where TEntity : class
{
    IQueryable<TEntity> DeferredSelectAll();
    TEntity Add(TEntity entity);
    Task<TEntity> AddAsync(TEntity entity, bool autoSave = true);
    Task<IEnumerable<TEntity>> AddAllAsync(IEnumerable<TEntity> entities, bool autoSave = true);
    Task<TEntity> FirstOrDefaultItemAsync(Expression<Func<TEntity, bool>> condition);

    IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition);

    IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition, string orderByProperties);

    IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition, int page, int pageSize);

    IQueryable<TEntity> DeferredWhere(Expression<Func<TEntity, bool>> condition, string orderByProperties, int page,
        int pageSize);

    IQueryable<DEntity> DeferredWhere<DEntity>(Expression<Func<DEntity, bool>> condition) where DEntity : TEntity;
    IQueryable<TEntity> DeferredWhereNoTracking(Expression<Func<TEntity, bool>> condition);


    TEntity Find(params object[] keyValue);

    //Task<TEntity> FindAsync(params object[] keyValue);

    bool Any(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> UpdateAsync(TEntity entity, bool autoSave);
    Task<IEnumerable<TEntity>> UpdateAllAsync(IEnumerable<TEntity> entities, bool autoSave = true);
    Task<TEntity> RemoveAsync(TEntity entity, bool autoSave);
    Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, bool autoSave);
    Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> condition = null);

    void Dispose();
}