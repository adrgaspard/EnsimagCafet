using APITools.CommonTools;
using Microsoft.EntityFrameworkCore;

namespace APITools.EntityFrameworkCore
{
    public sealed class TransientDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext
    {
        private readonly Func<TDbContext> _contextFactory;

        public TransientDbContextProvider(Func<TDbContext> contextFactory)
        {
            ArgumentNullException.ThrowIfNull(contextFactory, nameof(contextFactory));
            _contextFactory = contextFactory;
        }

        public Task<Result<TDbContext>> GetDbContextAsync()
        {
            try
            {
                return Task.FromResult(Result.Success(_contextFactory()));
            }
            catch (Exception exception)
            {
                return Task.FromResult(Result.Error<TDbContext>(exception));
            }
        }

        Task<Result<DbContext>> IDbContextProvider.GetDbContextAsync()
        {
            try
            {
                return Task.FromResult(Result.Success<DbContext>(_contextFactory()));
            }
            catch (Exception exception)
            {
                return Task.FromResult(Result.Error<DbContext>(exception));
            }
        }

    }
}
