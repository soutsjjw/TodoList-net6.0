using System.Linq.Expressions;

namespace TodoList.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    // Create相關操作接口
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    // Update相關操作接口
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    // Delete相關操作接口，這裡根據key刪除對象的接口需要用到一個獲取對象的方法
    ValueTask<T?> GetAsync(object key);
    Task DeleteAsync(object key);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    #region 1. 查詢基礎操作接口

    IQueryable<T> GetAsQueryable();
    IQueryable<T> GetAsQueryable(ISpecification<T> spec);

    #endregion

    #region 2. 查詢數量相關接口

    int Count(ISpecification<T>? spec = null);
    int Count(Expression<Func<T, bool>> condition);
    Task<int> CountAsync(ISpecification<T>? spec);

    #endregion

    #region 3. 查詢存在性相關接口

    bool Any(ISpecification<T>? spec);
    bool Any(Expression<Func<T, bool>>? condition = null);

    #endregion

    #region 4. 根據條件獲取原始實體類型數據相關接口

    Task<T?> GetAsync(Expression<Func<T, bool>> condition);
    Task<IReadOnlyList<T>> GetAsync();
    Task<IReadOnlyList<T>> GetAsync(ISpecification<T>? spec);

    #endregion

    #region 5. 根據條件獲取映射實體類型數據相關接口，涉及到Group相關操作也在其中，使用selector來傳入映射的表達式

    TResult? SelectFirstOrDefault<TResult>(ISpecification<T>? spec, Expression<Func<T, TResult>> selector);
    Task<TResult?> SelectFirstOrDefaultAsync<TResult>(ISpecification<T>? spec, Expression<Func<T, TResult>> selector);

    Task<IReadOnlyList<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector);
    Task<IReadOnlyList<TResult>> SelectAsync<TResult>(ISpecification<T>? spec, Expression<Func<T, TResult>> selector);
    Task<IReadOnlyList<TResult>> SelectAsync<TGroup, TResult>(Expression<Func<T, TGroup>> groupExpression, Expression<Func<IGrouping<TGroup, T>, TResult>> selector, ISpecification<T>? spec = null);

    #endregion
    
}