using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;

namespace Application.Utils;

public class BusinessLogicUtility
{
    public Expression CreatePropertyLambdaExpression<TEntity>(string property)
    {
        var type = typeof(TEntity);
        var arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        var pi = type.GetProperty(property);
        expr = Expression.Property(expr, pi ?? throw new InvalidOperationException());
        type = pi.PropertyType;
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
        var lambda = Expression.Lambda(delegateType, expr, arg);

        return lambda;
    }

    public List<Expression> CreatePropertiesLambdaExpressions<TEntity>(string[] properties)
    {
        return properties.Select(CreatePropertyLambdaExpression<TEntity>).ToList();
    }

    public string[] GetEntityProperties(object entity, params string[] exceptProperties)
    {
        // Implement this method without Where for more performance.
        var propertyInfos = entity.GetType().GetProperties() /*.Where(p => !p.GetGetMethod().IsVirtual)*/.ToArray();
        var properties = new string[propertyInfos.Length - exceptProperties.Length];
        byte counter = 0;
        foreach (var propertyInfo in propertyInfos)
        {
            var propertyName = propertyInfo.Name;
            if (!exceptProperties.Contains(propertyName) /* && propertyInfos[i].GetMethod.IsFinal*/)
                properties[counter++] = propertyName;
        }

        return properties;
    }

    /// <summary>
    ///     Do parallel operations in a thread till done or timeout.
    /// </summary>
    /// <param name="parallelOperationsMethod">An Action contains parallel operations.</param>
    /// <param name="millisecondsTimeout">Delay timeout between retries.</param>
    /// <param name="maxTryCount">Max try count, 0 means infinit try.</param>
    public void DoParallel(Action parallelOperationsMethod, int millisecondsTimeout = 0, int maxTryCount = 1)
    {
        Task.Run(() =>
        {
            var infinit = maxTryCount < 1;
            while (true)
                try
                {
                    //await Task.Factory.StartNew(parallelOperationsMethod);
                    var task = Task.Factory.StartNew(parallelOperationsMethod);
                    task.Wait();
                    break;
                }
#pragma warning disable CS0168
                // Variable is declared but never used
                catch (Exception exception)
#pragma warning restore CS0168
                    // Variable is declared but never used
                {
                    if (!infinit && --maxTryCount < 1) break;
                    //await Task.Delay(millisecondsTimeout);
                    Thread.Sleep(millisecondsTimeout);
                }
        });
    }

    /// <summary>
    ///     Do rolllback operations in a thread till done or timeout.
    /// </summary>
    /// <param name="rollBackOperationsMethod">An Action contains rollback operations.</param>
    /// <param name="millisecondsTimeout">Delay timeout between retries.</param>
    /// <param name="maxTryCount">Max try count, 0 means infinit try.</param>
    // public void RollBack(Action rollBackOperationsMethod,
    //     int millisecondsTimeout = BusinessLogicSetting.DefaultRollbackMillisecondsTimeout, int maxTryCount = 0)
    // {
    //     DoParallel(rollBackOperationsMethod, millisecondsTimeout, maxTryCount);
    // }
    public Task<TDestination> MapAsync<TSource, TDestination>(TSource source, TDestination destination)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.CreateMap<TSource, TDestination>();
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map(source, destination));
    }

    public Task<TDestination> MapAsync<TSource, TDestination>(
        Action<IMappingExpression<TSource, TDestination>> expression, TSource source, TDestination destination)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            expression(configuration.CreateMap<TSource, TDestination>());
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map(source, destination));
    }

    public Task<TDestination> MapAsync<TSource, TDestination>(TSource source)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.CreateMap<TSource, TDestination>();
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map<TDestination>(source));
    }

    public Task<TDestination> MapAsync<TSource, TDestination>(
        Action<IMappingExpression<TSource, TDestination>> expression, TSource source)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            expression(configuration.CreateMap<TSource, TDestination>());
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map<TDestination>(source));
    }

    public Task<TResult> MapAsync<TSource, TDestination, TResult>(object source, TResult destination)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.CreateMap<TSource, TDestination>();
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map(source, destination));
    }

    public Task<TResult> MapAsync<TSource, TDestination, TResult>(
        Action<IMappingExpression<TSource, TDestination>> expression, object source, TResult destination)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            expression(configuration.CreateMap<TSource, TDestination>());
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map(source, destination));
    }


    public Task<IQueryable<TDestination>> ProjectTo<TSource, TDestination>(IQueryable<TSource> source)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.CreateMap<TSource, TDestination>();
        });
        var mapper = mapperConfiguration.CreateMapper();

        return Task.Run(() => mapper.ProjectTo<TDestination>(source));
    }


    public Task<TResult> MapAsync<TSource, TDestination, TResult>(object source)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.CreateMap<TSource, TDestination>();
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map<TResult>(source));
    }

    public Task<TResult> MapAsync<TSource, TDestination, TResult>(
        Action<IMappingExpression<TSource, TDestination>> expression, object source)
    {
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            expression(configuration.CreateMap<TSource, TDestination>());
        });
        var mapper = mapperConfiguration.CreateMapper();
        return Task.Run(() => mapper.Map<TResult>(source));
    }

    public string GetDisplayName<T>(string propertyName)
    {
        string name;
        var type = typeof(T);
        if (type.BaseType == typeof(Enum))
            //var objectEnum = (Enum)obj;
            name = type.GetMember(propertyName).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.GetName();
        else
            name = type.GetProperty(propertyName)?.GetCustomAttribute<DisplayAttribute>()?.GetName();

        return name ?? propertyName;
    }

    public string GetDisplayName(Type type, string propertyName)
    {
        string name;
        if (type.BaseType == typeof(Enum))
            name = type.GetMember(propertyName).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.GetName();
        else
            name = type.GetProperty(propertyName)?.GetCustomAttribute<DisplayAttribute>()?.GetName();

        return name ?? propertyName;
    }

    public string GetDisplayName<T>(T obj)
    {
        string name;
        var type = typeof(T);
        if (type.BaseType == typeof(Enum))
            name = type.GetMember(obj.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? obj.ToString();
        else
            name = type.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? type.Name;

        return name;
    }

    public string GetDisplayName(Type type)
    {
        return type?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? type?.Name;
    }

    public object GetEntityReadyToUpdate(object model, object viewModel)
    {
        var viewModelProperties = viewModel.GetType().GetProperties();
        var modelProperties = model.GetType().GetProperties();

        foreach (var property in viewModelProperties)
        {
            var pro = modelProperties.Where(x => x.Name == property.Name && x.PropertyType == property.PropertyType)
                .FirstOrDefault();
            if (pro != null)
            {
                var newvalue = property.GetValue(viewModel);
                pro.SetValue(model, newvalue);
            }
        }

        return model;
    }
}