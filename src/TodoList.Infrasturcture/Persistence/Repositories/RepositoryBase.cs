using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoList.Application.Common;
using TodoList.Application.Common.Interfaces;

namespace TodoList.Infrasturcture.Persistence.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    private readonly TodoListDbContext _dbContext;

    public RepositoryBase(TodoListDbContext dbContext) => _dbContext = dbContext;

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        // 對於一般的更新而言，都是Attach到實體上的，只需要設置該實體的State為Modified就可以了
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual ValueTask<T?> GetAsync(object key) => _dbContext.Set<T>().FindAsync(key);

    public async Task DeleteAsync(object key)
    {
        var entity = await GetAsync(key);
        if (entity is not null)
        {
            await DeleteAsync(entity);
        }
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #region 1. 查詢基礎操作接口實現

    public IQueryable<T> GetAsQueryable()
        => _dbContext.Set<T>();

    public IQueryable<T> GetAsQueryable(ISpecification<T> spec)
        => ApplySpecification(spec);
    public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> condition)
        => _dbContext.Set<T>().Where(condition);

    #endregion

    #region 2. 查詢數量相關接口實現

    public int Count(Expression<Func<T, bool>> condition)
        => _dbContext.Set<T>().Count(condition);

    public int Count(ISpecification<T>? spec = null)
        => null != spec ? ApplySpecification(spec).Count() : _dbContext.Set<T>().Count();

    public Task<int> CountAsync(ISpecification<T>? spec)
        => ApplySpecification(spec).CountAsync();

    #endregion

    #region 3. 查詢存在性相關接口實現

    public bool Any(ISpecification<T>? spec)
        => ApplySpecification(spec).Any();

    public bool Any(Expression<Func<T, bool>>? condition = null)
        => null != condition ? _dbContext.Set<T>().Any(condition) : _dbContext.Set<T>().Any();

    #endregion

    #region 4. 根據條件獲取原始實體類型數據相關接口實現

    public async Task<T?> GetAsync(Expression<Func<T, bool>> condition)
        => await _dbContext.Set<T>().FirstOrDefaultAsync(condition);

    public async Task<IReadOnlyList<T>> GetAsync()
        => await _dbContext.Set<T>().AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T>? spec)
        => await ApplySpecification(spec).AsNoTracking().ToListAsync();

    #endregion

    #region 5. 根據條件獲取映射實體類型數據相關接口實現

    public TResult? SelectFirstOrDefault<TResult>(ISpecification<T>? spec, Expression<Func<T, TResult>> selector)
        => ApplySpecification(spec).AsNoTracking().Select(selector).FirstOrDefault();

    public Task<TResult?> SelectFirstOrDefaultAsync<TResult>(ISpecification<T>? spec, Expression<Func<T, TResult>> selector)
        => ApplySpecification(spec).AsNoTracking().Select(selector).FirstOrDefaultAsync();

    public async Task<IReadOnlyList<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector)
        => await _dbContext.Set<T>().AsNoTracking().Select(selector).ToListAsync();

    public async Task<IReadOnlyList<TResult>> SelectAsync<TResult>(ISpecification<T>? spec, Expression<Func<T, TResult>> selector)
        => await ApplySpecification(spec).AsNoTracking().Select(selector).ToListAsync();

    public async Task<IReadOnlyList<TResult>> SelectAsync<TGroup, TResult>(Expression<Func<T, TGroup>> groupExpression, Expression<Func<IGrouping<TGroup, T>, TResult>> selector, ISpecification<T>? spec = null)
        => null != spec ?
        await ApplySpecification(spec).AsNoTracking().GroupBy(groupExpression).Select(selector).ToListAsync() :
        await _dbContext.Set<T>().AsNoTracking().GroupBy(groupExpression).Select(selector).ToListAsync();


    #endregion

    // 用於拼接所有Specification的輔助方法，接收一個`IQuerybale<T>對象（通常是數據集合）
    // 和一個當前實體定義的Specification對象，並返回一個`IQueryable<T>`對象為子句執行後的結果。
    private IQueryable<T> ApplySpecification(ISpecification<T>? spec)
        => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
}
