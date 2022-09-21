using APITools.CommonTools;
using Microsoft.EntityFrameworkCore;

namespace APITools.EntityFrameworkCore
{
    public interface IDbContextProvider
    {
        Task<Result<DbContext>> GetDbContextAsync();
    }

    public interface IDbContextProvider<TDbContext> : IDbContextProvider where TDbContext : DbContext
    {
        new Task<Result<TDbContext>> GetDbContextAsync();
    }
}