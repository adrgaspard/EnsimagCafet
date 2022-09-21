using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using System.Reflection;

namespace EnsimagCafet.EntityFrameworkCore.DbMigrator
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string connectionName = args.Length == 0 ? "DefaultConnection" : string.Join(" ", args);
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(new() { ExeConfigFilename = "App.config" }, ConfigurationUserLevel.None);
            string connectionString = config.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString;
            return new(new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString, builder =>
            {
                _ = builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            }).Options);
        }
    }
}
