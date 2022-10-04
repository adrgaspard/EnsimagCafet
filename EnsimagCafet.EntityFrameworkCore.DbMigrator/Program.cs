using EnsimagCafet.EntityFrameworkCore.DbMigrator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Seeding database...");

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", true, true)
    .AddJsonFile("appsettings.Production.json", true, true)
    .AddEnvironmentVariables()
    .AddCommandLine(Array.Empty<string>())
    .Build();

using var context = new ApplicationDbContextFactory(config).CreateDbContext(Array.Empty<string>());

context.Database.Migrate();

new ApplicationDbDataSeeder(new() { SuperUserDefaultPassword = Environment.GetEnvironmentVariable("SU_DEFAULT_PASSWORD") ?? config.GetSection("Seeding").GetSection("SuperUserDefaultPassword").Value }).Seed(context);
