using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace TodoList.Application.Common.Interfaces;

public interface ISpecification<T>
{
    // 查詢條件子句
    Expression<Func<T, bool>> Criteria { get; }
    // Include子句
    Func<IQueryable<T>, IIncludableQueryable<T, object>> Include { get; }
    // OrderBy子句
    Expression<Func<T, object>> OrderBy { get; }
    // OrderByDescending子句
    Expression<Func<T, object>> OrderByDescending { get; }

    // 分頁相關屬性
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
