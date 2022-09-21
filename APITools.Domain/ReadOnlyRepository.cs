using APITools.CommonTools;
using APITools.Domain.Contracts;
using System.Data;
using System.Linq.Expressions;

namespace APITools.Domain
{
    public sealed class ReadOnlyRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        private static readonly Result WriteOperationInvalidResult = Result.Error(new ReadOnlyException("This repository is read-only."));

        private static readonly Result<TEntity> WriteOperationInvalidTypedResult = Result.Error<TEntity>(new ReadOnlyException("This repository is read-only."));

        private readonly IReadOnlyRepository<TEntity, TKey> _repository;

        public ReadOnlyRepository(IReadOnlyRepository<TEntity, TKey> repository)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            _repository = repository;
        }

        public Task<Result> DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }

        public Task<Result> DeleteOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }

        public async Task<Result<long>> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.GetCountAsync(cancellationToken);
        }

        public async Task<Result<long>> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _repository.GetCountAsync(predicate, cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(skipCount, maxResultCount, cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(predicate, cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(sort, cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(skipCount, maxResultCount, predicate, cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(skipCount, maxResultCount, sort, cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(predicate, sort, cancellationToken);
        }

        public async Task<Result<IList<TEntity>>> GetManyAsync(int skipCount, int maxResultCount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IComparable>> sort, CancellationToken cancellationToken = default)
        {
            return await _repository.GetManyAsync(skipCount, maxResultCount, predicate, sort, cancellationToken);
        }

        public async Task<Result<TEntity>> GetOneAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetOneAsync(id, cancellationToken);
        }

        public async Task<Result<TEntity>> GetOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _repository.GetOneAsync(predicate, cancellationToken);
        }

        public async Task<Result<TEntity?>> GetOneOrDefaultAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetOneOrDefaultAsync(id, cancellationToken);
        }

        public async Task<Result<TEntity?>> GetOneOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _repository.GetOneOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<Result<TEntity>> GetSingleAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetSingleAsync(id, cancellationToken);
        }

        public async Task<Result<TEntity>> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _repository.GetSingleAsync(predicate, cancellationToken);
        }

        public async Task<Result<TEntity?>> GetSingleOrDefaultAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetSingleOrDefaultAsync(id, cancellationToken);
        }

        public async Task<Result<TEntity?>> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _repository.GetSingleOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<Result> HardDeleteOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }

        public Task<Result> HardDeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }

        public Task<Result> InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }

        public Task<Result<TEntity>> InsertOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidTypedResult);
        }

        public Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }

        public Task<Result> UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }

        public Task<Result> UpdateOneAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(WriteOperationInvalidResult);
        }
    }
}
