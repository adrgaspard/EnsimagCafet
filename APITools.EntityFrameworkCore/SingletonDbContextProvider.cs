using APITools.CommonTools;
using Microsoft.EntityFrameworkCore;

namespace APITools.EntityFrameworkCore
{
    public sealed class SingletonDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext
    {
        private readonly Result<DbContext> _genericContextResult;
        private readonly Result<TDbContext> _contextResult;

        public SingletonDbContextProvider(TDbContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            _genericContextResult = context;
            _contextResult = context;
        }

        public Task<Result<TDbContext>> GetDbContextAsync()
        {
            return Task.FromResult(_contextResult);
        }

        Task<Result<DbContext>> IDbContextProvider.GetDbContextAsync()
        {
            return Task.FromResult(_genericContextResult);
        }
    }
}