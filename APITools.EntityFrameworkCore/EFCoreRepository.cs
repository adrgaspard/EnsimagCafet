using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APITools.EntityFrameworkCore
{
    public sealed class EFCoreRepository<TEntity, TKey> : Repository<TEntity, TKey> where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        private const string NullDbContext = "The DbContext was null.";
        private readonly IDbContextProvider _contextProvider;

        public EFCoreRepository(IDbContextProvider contextProvider)
        {
            ArgumentNullException.ThrowIfNull(contextProvider, nameof(contextProvider));
            _contextProvider = contextProvider;
        }

        public override async Task<Result<TEntity>> GetOneAsync(TKey id, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.FirstAsync(entity => entity.Id.Equals(id), cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity>> GetOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.FirstAsync(predicate, cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity?>> GetOneOrDefaultAsync(TKey id, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity?>> GetOneOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.FirstOrDefaultAsync(predicate, cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity>> GetSingleAsync(TKey id, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.SingleAsync(entity => entity.Id.Equals(id), cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity>> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.SingleAsync(predicate, cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity?>> GetSingleOrDefaultAsync(TKey id, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.SingleOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity?>> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.SingleOrDefaultAsync(predicate, cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.Skip(skipCount).Take(maxResultCount).ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.Where(predicate).ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.OrderBy(sort).ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.Where(predicate).Skip(skipCount).Take(maxResultCount).ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.OrderBy(sort).Skip(skipCount).Take(maxResultCount).ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.Where(predicate).OrderBy(sort).ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.Where(predicate).OrderBy(sort).Skip(skipCount).Take(maxResultCount).ToListAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<long>> GetCountAsync(CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.CountAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<long>> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    return await set.CountAsync(predicate, cancellationToken);
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result<TEntity>> InsertOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    _ = await set.AddAsync(entity, cancellationToken);
                    _entityTypeCreationTimeProperty?.SetValue(entity, DateTime.Now);
                    _entityTypeModificationTimeProperty?.SetValue(entity, null);
                    _entityTypeIsDeletedProperty?.SetValue(entity, false);
                    _entityTypeDeletionTimeProperty?.SetValue(entity, null);
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    await set.AddRangeAsync(entities, cancellationToken);
                    foreach (TEntity entity in entities)
                    {
                        _entityTypeCreationTimeProperty?.SetValue(entity, DateTime.Now);
                        _entityTypeModificationTimeProperty?.SetValue(entity, null);
                        _entityTypeIsDeletedProperty?.SetValue(entity, false);
                        _entityTypeDeletionTimeProperty?.SetValue(entity, null);
                    }
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> UpdateOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    _ = set.Update(entity);
                    _entityTypeModificationTimeProperty?.SetValue(entity, DateTime.UtcNow);
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    set.UpdateRange(entities);
                    foreach (TEntity entity in entities)
                    {
                        _entityTypeModificationTimeProperty?.SetValue(entity, DateTime.UtcNow);
                    }
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> DeleteOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    if (_entityTypeIsDeletedProperty is null)
                    {
                        _ = set.Remove(entity);
                    }
                    else
                    {
                        _entityTypeIsDeletedProperty.SetValue(entity, true);
                        _entityTypeDeletionTimeProperty?.SetValue(entity, DateTime.UtcNow);
                    }
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    if (_entityTypeIsDeletedProperty is null)
                    {
                        set.RemoveRange(entities);
                    }
                    else
                    {
                        foreach (TEntity entity in entities)
                        {
                            _entityTypeIsDeletedProperty.SetValue(entity, true);
                            _entityTypeDeletionTimeProperty?.SetValue(entity, DateTime.UtcNow);
                        }
                    }
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> HardDeleteOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    set.RemoveRange(entity);
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> HardDeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    DbSet<TEntity> set = context.Set<TEntity>();
                    set.RemoveRange(entities);
                    if (autoSave)
                    {
                        _ = await context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }

        public override async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Result<DbContext> fetchContext = await _contextProvider.GetDbContextAsync();
            if (fetchContext.IsSuccess)
            {
                DbContext context = fetchContext.Value;
                try
                {
                    _ = await context.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            return new NullReferenceException(NullDbContext, fetchContext.Exception);
        }
    }
}