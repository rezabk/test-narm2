using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

public static class RepositoryExtension
{
    private const char PropertySeprator = ',';
    private const char PropertyValueSeprator = ':';

    #region Filter

    public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> source,
        Expression<Func<TEntity, bool>> filterExpression)
    {
        return source.Where(filterExpression).AsQueryable();
    }

    #endregion

    #region Paginate

    public static IQueryable<TEntity> DeferredPaginate<TEntity>(this IQueryable<TEntity> entities, int page,
        int pageSize)
    {
        return entities.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static IList<TEntity> Paginate<TEntity>(this IQueryable<TEntity> entities, int page, int pageSize)
    {
        return entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public static async Task<IList<TEntity>> PaginateAsync<TEntity>(this IQueryable<TEntity> entities, int page,
        int pageSize)
    {
        return await entities.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    #endregion

    #region OrderBy

    // TODO: یکسان سازی این تکنولوژی برای جستجو و مرتب سازی در ریپوزیتوری بیس.
    private static IOrderedQueryable<TEntity> ApplyOrder<TEntity>(IQueryable<TEntity> source, string property,
        string methodName)
    {
        //string[] props = property.Split('.');
        var type = typeof(TEntity);
        var arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        //foreach (string prop in props)
        //{
        // use reflection (not ComponentModel) to mirror LINQ
        var pi = type.GetProperty(property);
        expr = Expression.Property(expr, pi);
        type = pi.PropertyType;
        //}
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
        var lambda = Expression.Lambda(delegateType, expr, arg);

        var result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TEntity), type)
            .Invoke(null, new object[] { source, lambda });
        return (IOrderedQueryable<TEntity>)result;
    }

    private static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string property)
    {
        return ApplyOrder(source, property, "OrderBy");
    }

    private static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source,
        string property)
    {
        return ApplyOrder(source, property, "OrderByDescending");
    }

    private static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string property)
    {
        return ApplyOrder(source, property, "ThenBy");
    }

    private static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source,
        string property)
    {
        return ApplyOrder(source, property, "ThenByDescending");
    }

    private static LambdaExpression CreatePropertyLambdaExpression<TEntity>(string property)
    {
        var type = typeof(TEntity);
        var arg = Expression.Parameter(type, "x"); // x
        Expression expr = arg;
        var pi = type.GetProperty(property);
        expr = Expression.Property(expr, pi); // x.Id
        type = pi.PropertyType;
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
        var lambda = Expression.Lambda(delegateType, expr, arg); // x => x.Id

        return lambda;
    }

    private static Expression<Func<TEntity, bool>> CreateEqualComparisonLambdaExperession<TEntity>(string property,
        string value)
    {
        var type = typeof(TEntity);
        var parameter = Expression.Parameter(type, "x"); // x
        Expression expr = parameter;
        var pi = type.GetProperty(property);
        expr = Expression.Property(expr, pi); // x.Id
        type = pi.PropertyType;
        var constant = Expression.Parameter(type, value); // 3
        var body = Expression.Equal(expr, constant); // x.Id >= 3
        var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter); // x => x.Id >= 3
        return lambda;
    }

    private static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> source, string property,
        string value)
    {
        return source.Where(CreateEqualComparisonLambdaExperession<TEntity>(property, value));
    }

    private static IQueryable<TEntity> ApplyAllFilters<TEntity>(this IQueryable<TEntity> source, string properties)
    {
        var splitedProperties = properties.Split(PropertySeprator);
        if (!splitedProperties.Any())
            return source;
        for (var i = 0; i < splitedProperties.Length; i++)
        {
            var property = splitedProperties[i].Split(PropertyValueSeprator);
            if (property.Count() != 2)
                return source;
            var propertyName = property[0];
            var filterValue = property[1];
            source = source.ApplyFilter(propertyName, filterValue);
        }

        return source;
    }

    public static IOrderedQueryable<TEntity> ApplyAllOrderBy<TEntity>(this IQueryable<TEntity> source,
        string properties)
    {
        var orderedSource = (IOrderedQueryable<TEntity>)source;
        var splitedProperties = properties.Split(PropertySeprator);
        if (!splitedProperties.Any())
            return orderedSource;
        var firstProperty = splitedProperties[0].Split(PropertyValueSeprator);
        if (firstProperty.Count() != 2)
            return orderedSource;
        var firstPropertyName = firstProperty[0];
        var firstPropertySortOrder = firstProperty[1];
        if (firstPropertySortOrder == "Asc")
            orderedSource = orderedSource.OrderBy(firstPropertyName);
        else if (firstPropertySortOrder == "Desc")
            orderedSource = orderedSource.OrderByDescending(firstPropertyName);
        else
            return orderedSource;
        for (var i = 1; i < splitedProperties.Length; i++)
        {
            var property = splitedProperties[i].Split(PropertyValueSeprator);
            if (property.Count() != 2)
                return orderedSource;
            var propertyName = property[0];
            var sortOrder = property[1];
            if (sortOrder == "Asc")
                orderedSource = orderedSource.ThenBy(propertyName);
            else if (sortOrder == "Desc")
                orderedSource = orderedSource.ThenByDescending(propertyName);
            else
                return orderedSource;
        }

        return orderedSource;
    }

    #endregion
}