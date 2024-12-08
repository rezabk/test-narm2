using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common;

public static class Pagination
{
    public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int pageSize,
        out int rowsCount)
    {
        rowsCount = source.Count();
        return source.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, IPaginate pagination,
        out int totalCount)
    {
        totalCount = source.Count();
        return source.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
    }

    public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, IPaginate pagination)
    {
        return source.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
    }

    public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, IPaginate pagination,
        out int totalCount)
    {
        totalCount = source.Count();
        return source.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
    }

    public static IQueryable<TSource> OrderByDescendingPaginate<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, object>> condition, IPaginate pagination)
    {
        return source.OrderByDescending(condition).Paginate(pagination);
    }

    public static IQueryable<TSource> OrderByDescendingPaginate<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, object>> condition, IPaginate pagination, out int totalCount)
    {
        totalCount = source.Count();
        return source.OrderByDescending(condition).Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
    }

    public static IEnumerable<TSource> OrderByDescendingPaginate<TSource>(this IEnumerable<TSource> source,
        Func<TSource, object> condition, IPaginate pagination, out int totalCount)
    {
        totalCount = source.Count();
        return source.OrderByDescending(condition).Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
    }
}