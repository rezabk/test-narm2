using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Application.IRepositories;
using Application.Services.Base;
using Common.Enums.UserEnum;
using Domain.Attributes;
using Domain.Entities.UserAgg;
using Infrastructure.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.ContextConfig.OnModelCreatingConfigs;

namespace Persistence.Contexts;

public class ApplicationDataBaseContext :
    IdentityDbContext<ApplicationUser, ApplicationRole, int>,
    IMyContext,
    IUnitOfWork
{
    private readonly IHttpContextAccessor _context;

    private IDictionary<Type, object> _repositories;

    public ApplicationDataBaseContext(
        DbContextOptions<ApplicationDataBaseContext> options,
        IHttpContextAccessor context) : base(options)
    {
        _context = context;
    }

    public override int SaveChanges()
    {
        #region SaveAuditAble

        var modifiedEntitis = ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Added ||
                        p.State == EntityState.Modified ||
                        p.State == EntityState.Deleted);

        foreach (var item in modifiedEntitis)
        {
            var entity = item.Context.Model.FindEntityType(item.Entity.GetType());
            if (entity == null) continue;

            var insert = entity.FindProperty("InsertTime");
            var insertbyuserid = entity.FindProperty("InsertByUserId");
            var update = entity.FindProperty("UpdateTime");
            var updatebyuserid = entity.FindProperty("UpdateByUserId");
            var remove = entity.FindProperty("RemoveTime");
            var removebyuserid = entity.FindProperty("RemoveByUserId");
            var isremoved = entity.FindProperty("IsRemoved");

            if (item.State == EntityState.Added && insert != null)
            {
                item.Property("InsertTime").CurrentValue = DateTime.UtcNow;
                item.Property("IsRemoved").CurrentValue = false;
                item.Property("InsertByUserId").CurrentValue = _context.HttpContext?.User?.Identity?.Name;
            }

            if (item.State == EntityState.Modified && update != null)
            {
                item.Property("UpdateTime").CurrentValue = DateTime.UtcNow;
                item.Property("UpdateByUserId").CurrentValue = _context.HttpContext?.User?.Identity?.Name;
            }

            if (item.State == EntityState.Deleted && remove != null && isremoved != null)
            {
                item.Property("RemoveTime").CurrentValue = DateTime.UtcNow;
                item.Property("RemoveByUserId").CurrentValue = _context.HttpContext?.User?.Identity?.Name;
                item.Property("IsRemoved").CurrentValue = true;
                item.State = EntityState.Modified;
            }
        }

        #endregion

        return base.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        #region SaveAuditAble

        var modifiedEntitis = ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Added ||
                        p.State == EntityState.Modified ||
                        p.State == EntityState.Deleted);

        foreach (var item in modifiedEntitis)
        {
            var entity = item.Context.Model.FindEntityType(item.Entity.GetType());
            if (entity == null) continue;

            var insert = entity.FindProperty("InsertTime");
            var insertbyuserid = entity.FindProperty("InsertByUserId");
            var update = entity.FindProperty("UpdateTime");
            var updatebyuserid = entity.FindProperty("UpdateByUserId");
            var remove = entity.FindProperty("RemoveTime");
            var removebyuserid = entity.FindProperty("RemoveByUserId");
            var isremoved = entity.FindProperty("IsRemoved");

            if (item.State == EntityState.Added && insert != null)
            {
                item.Property("InsertTime").CurrentValue = DateTime.UtcNow;
                item.Property("IsRemoved").CurrentValue = false;
                try
                {
                    item.Property("InsertByUserId").CurrentValue =
                        _context.HttpContext?.User?.Identities?
                            .FirstOrDefault()?.Claims?
                            .FirstOrDefault(x =>
                                x.Type == nameof(ClaimUserEnum.userid))?.Value;
                }
                catch
                {
                    item.Property("InsertByUserId").CurrentValue = null;
                }
            }

            if (item.State == EntityState.Modified && update != null)
            {
                item.Property("UpdateTime").CurrentValue = DateTime.UtcNow;
                try
                {
                    item.Property("UpdateByUserId").CurrentValue =
                        _context.HttpContext?.User?.Identities?
                            .FirstOrDefault()?.Claims?
                            .FirstOrDefault(x =>
                                x.Type == nameof(ClaimUserEnum.userid))?.Value;
                }
                catch
                {
                    item.Property("UpdateByUserId").CurrentValue = null;
                }
            }

            if (item.State == EntityState.Deleted && remove != null && isremoved != null)
            {
                item.Property("RemoveTime").CurrentValue = DateTime.UtcNow;
                try
                {
                    item.Property("RemoveByUserId").CurrentValue =
                        _context.HttpContext?.User?.Identities?
                            .FirstOrDefault()?.Claims?
                            .FirstOrDefault(x =>
                                x.Type == nameof(ClaimUserEnum.userid))?.Value;
                }
                catch
                {
                    item.Property("RemoveByUserId").CurrentValue = null;
                }

                item.Property("IsRemoved").CurrentValue = true;
                item.State = EntityState.Modified;
            }
        }

        #endregion

        return base.SaveChangesAsync();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories == null)
            _repositories = new Dictionary<Type, object>();

        var entityType = typeof(TEntity);
        if (_repositories.Keys.Contains(entityType)) return _repositories[entityType] as IRepository<TEntity>;

        IRepository<TEntity> repository = new Repository<TEntity>(this);
        _repositories.Add(entityType, repository);
        return repository;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Config(builder);
        base.OnModelCreating(builder);


        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDataBaseContext).Assembly);
    }

    private void Config(ModelBuilder builder)
    {
        MigarationAndUpdateDatabaseEntities(builder);

        //Reflection Shadow Property For Auditable Filed
        ReflectionEntitis.Execute(builder);

        var entityTypes = builder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            // // Skip entities
            // if
            // (
            // )
            //     continue;

            var isActiveProperty = entityType.FindProperty("IsRemoved");
            if (isActiveProperty != null)
            {
                var entityBuilder = builder.Entity(entityType.ClrType);
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var methodInfo = typeof(EF).GetMethod(nameof(EF.Property))!.MakeGenericMethod(typeof(bool))!;
                var efPropertyCall = Expression.Call(null, methodInfo, parameter, Expression.Constant("IsRemoved"));
                var body = Expression.MakeBinary(ExpressionType.Equal, efPropertyCall, Expression.Constant(false));
                var expression = Expression.Lambda(body, parameter);
                entityBuilder.HasQueryFilter(expression);
            }
        }
    }

    private static void MigarationAndUpdateDatabaseEntities(ModelBuilder modelBuilder)
    {
        var asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + nameof(Domain) +
                      ".dll";
        var modelInAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(asmPath);
        var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
        foreach (var type in modelInAssembly.ExportedTypes)
        {
            var typeFind =
                type.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == nameof(EntityTypeAttribute));
            if (typeFind != null)
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
        }
    }

    private void SetQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var isActiveProperty = entityType.FindProperty("IsRemoved");
            if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var filter = Expression.Lambda(Expression.Property(parameter, isActiveProperty.PropertyInfo),
                    parameter);
                entityType.SetQueryFilter(filter);
            }
        }
    }
}

public static class ModelBuilderExtension
{
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder,
        Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);
        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newbody =
                ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
        }
    }
}