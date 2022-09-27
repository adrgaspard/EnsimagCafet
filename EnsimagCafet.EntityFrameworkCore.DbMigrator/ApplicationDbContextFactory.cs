using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using System.Reflection;

namespace EnsimagCafet.EntityFrameworkCore.DbMigrator
{
    public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private readonly Configuration _configuration;

        public ApplicationDbContextFactory() : this(ConfigurationManager.OpenMappedExeConfiguration(new() { ExeConfigFilename = "App.config" }, ConfigurationUserLevel.None))

        public ApplicationDbContextFactory(Configuration configuration)
        {
            _configuration = configuration;
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string connectionName = args.Length == 0 ? "DefaultConnection" : string.Join(" ", args);
            string connectionString = _configuration.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString;
            return new(new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString, builder =>
            {
                _ = builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            }).Options);
        }
    }
}