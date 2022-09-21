using APITools.CommonTools;
using APITools.Domain.Contracts;
using APITools.Domain.Contracts.Auditing;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace APITools.Domain
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        protected static readonly PropertyInfo? _entityTypeCreationTimeProperty;
        protected static readonly PropertyInfo? _entityTypeModificationTimeProperty;
        protected static readonly PropertyInfo? _entityTypeIsDeletedProperty;
        protected static readonly PropertyInfo? _entityTypeDeletionTimeProperty;

        static Repository()
        {
            Type entityType = typeof(TEntity);
            _entityTypeCreationTimeProperty = entityType.IsAssignableFrom(typeof(IHasCreationTime)) ? entityType.GetProperty(nameof(IHasCreationTime.CreationTime)) : null;
            _entityTypeModificationTimeProperty = entityType.IsAssignableFrom(typeof(IHasModificationTime)) ? entityType.GetProperty(nameof(IHasModificationTime.LastModificationTime)) : null;
            _entityTypeIsDeletedProperty = entityType.IsAssignableFrom(typeof(ISoftDelete)) ? entityType.GetProperty(nameof(ISoftDelete.IsDeleted)) : null;
            _entityTypeDeletionTimeProperty = entityType.IsAssignableFrom(typeof(IHasDeletionTime)) ? entityType.GetProperty(nameof(IHasDeletionTime.DeletionTime)) : null;
            foreach (PropertyInfo? prop in new List<PropertyInfo?>(4) { _entityTypeCreationTimeProperty, _entityTypeModificationTimeProperty, _entityTypeIsDeletedProperty, _entityTypeDeletionTimeProperty })
            {
                if (prop is PropertyInfo property && !property.CanWrite)
                {
                    throw new ReadOnlyException($"You must define the property {entityType.Name}.{property.Name} setter (it can be private but can't be an init).");
                }
            }
        }

        public abstract Task<Result> DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<Result> DeleteOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<Result<long>> GetCountAsync(CancellationToken cancellationToken = default);

        public abstract Task<Result<long>> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default);

        public abstract Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity>> GetOneAsync(TKey id, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity>> GetOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity?>> GetOneOrDefaultAsync(TKey id, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity?>> GetOneOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity>> GetSingleAsync(TKey id, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity>> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity?>> GetSingleOrDefaultAsync(TKey id, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity?>> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task<Result> HardDeleteOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<Result> HardDeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<Result> InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<Result<TEntity>> InsertOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);

        public abstract Task<Result> UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<Result> UpdateOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}
