using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EnsimagCafet.EntityFrameworkCore.DbMigrator
{
    public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContextFactory() : this(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.Development.json", true, true)
            .AddJsonFile("appsettings.Production.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine(Array.Empty<string>())
            .Build())
        {
        }

        public ApplicationDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string connectionName = args.Length == 0 ? "DefaultConnection" : string.Join(" ", args);
            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? _configuration.GetConnectionString(connectionName);
            return new(new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString, builder =>
            {
                _ = builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            }).Options);
        }
    }
}