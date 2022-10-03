using EnsimagCafet.EntityFrameworkCore.DbMigrator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).AddEnvironmentVariables().AddCommandLine(Array.Empty<string>()).Build();

using var context = new ApplicationDbContextFactory(config).CreateDbContext(Array.Empty<string>());

context.Database.Migrate();

new ApplicationDbDataSeeder(new() { SuperUserDefaultPassword = Environment.GetEnvironmentVariable("SU_DEFAULT_PASSWORD") ?? config.GetSection("Seeding").GetSection("SuperUserDefaultPassword").Value }).Seed(context);